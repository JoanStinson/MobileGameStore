using JGM.GameStore.Packs.Data;
using JGM.GameStore.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JGM.GameStore.Packs
{
    public sealed class StorePacksController : MonoBehaviour, IStorePacksController
    {
        public class ShopPackEvent : UnityEvent<StorePack> { }
        public ShopPackEvent OnPackActivated = new ShopPackEvent();
        public ShopPackEvent OnPackRemoved = new ShopPackEvent();

        public List<StorePack> ActivePacks { get; private set; }

        private const float _refreshFrequency = 1f;
        private const int _numberOfActiveOfferPacks = 3;
        private const int _offersHistoryMaxSize = _numberOfActiveOfferPacks + 1;

        private List<StorePack> _activeOfferPacks;
        private List<StorePackData> _offerPacksDatabase;
        private Queue<string> _offerPacksHistory;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            InvokeRepeating("UpdatePeriodic", 0f, _refreshFrequency);
        }

        private void UpdatePeriodic()
        {
            Refresh();
        }

        private void Refresh()
        {
            var packsToRemove = new List<StorePack>();

            for (int i = 0; i < ActivePacks.Count; ++i)
            {
                ActivePacks[i].CheckExpiration();
                bool needsToBeRemoved = (ActivePacks[i].PackState == StorePack.State.Expired);
                if (needsToBeRemoved)
                {
                    packsToRemove.Add(ActivePacks[i]);
                }
            }

            for (int i = 0; i < packsToRemove.Count; ++i)
            {
                RemovePack(packsToRemove[i]);
            }

            int loopCount = 50;
            while (_activeOfferPacks.Count < _numberOfActiveOfferPacks && loopCount > 0)
            {
                loopCount--;

                var poolOfSelectablePacks = new List<StorePackData>();
                for (int i = 0; i < _offerPacksDatabase.Count; ++i)
                {
                    if (_offerPacksHistory.Contains(_offerPacksDatabase[i].Id))
                    {
                        continue;
                    }

                    poolOfSelectablePacks.Add(_offerPacksDatabase[i]);
                }

                bool anyValidCandidates = (poolOfSelectablePacks.Count > 0);
                if (anyValidCandidates)
                {
                    int randomPackIndex = Random.Range(0, poolOfSelectablePacks.Count);
                    var newPackData = poolOfSelectablePacks[randomPackIndex];
                    CreateAndActivatePack(newPackData);
                }
                else
                {
                    _offerPacksHistory.Dequeue();
                }
            }
        }

        private void CreateAndActivatePack(StorePackData storePackData)
        {
            var storePack = StorePack.CreateFromData(storePackData);
            ActivePacks.Add(storePack);

            if (storePack.PackData.PackType == StorePackData.Type.Offer)
            {
                _activeOfferPacks.Add(storePack);
                _offerPacksHistory.Enqueue(storePackData.Id);

                while (_offerPacksHistory.Count > _offersHistoryMaxSize)
                {
                    _offerPacksHistory.Dequeue();
                }
            }

            storePack.Activate();
            OnPackActivated?.Invoke(storePack);
        }

        private void RemovePack(StorePack storePack)
        {
            ActivePacks.Remove(storePack);
            _activeOfferPacks.Remove(storePack);
            OnPackRemoved?.Invoke(storePack);
        }

        private void Initialize()
        {
            ActivePacks = new List<StorePack>();
            _activeOfferPacks = new List<StorePack>();
            _offerPacksDatabase = new List<StorePackData>();
            _offerPacksHistory = new Queue<string>();

            var storeText = Resources.Load<TextAsset>("Data/shop_manager");
            var storeJson = JSONNode.Parse(storeText.text);

            _offerPacksDatabase.Clear();
            _activeOfferPacks.Clear();
            ActivePacks.Clear();

            if (storeJson.HasKey("packs"))
            {
                var packsData = storeJson["packs"].AsArray;
                for (int i = 0; i < packsData.Count; ++i)
                {
                    var storePackData = StorePackData.CreateFromJson(packsData[i]);
                    if (storePackData.PackType != StorePackData.Type.Offer)
                    {
                        CreateAndActivatePack(storePackData);
                    }
                    else
                    {
                        _offerPacksDatabase.Add(storePackData);
                    }
                }
            }
        }
    }
}