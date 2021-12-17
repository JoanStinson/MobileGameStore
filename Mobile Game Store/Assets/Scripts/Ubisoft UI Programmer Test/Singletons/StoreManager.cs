﻿using System.Collections.Generic;
using Ubisoft.UIProgrammerTest.Data;
using Ubisoft.UIProgrammerTest.Logic;
using Ubisoft.UIProgrammerTest.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ubisoft.UIProgrammerTest.Singletons
{
    public class StoreManager : MonoBehaviour
    {
        public class ShopPackEvent : UnityEvent<StorePack> { }
        public ShopPackEvent OnPackActivated = new ShopPackEvent();
        public ShopPackEvent OnPackRemoved = new ShopPackEvent();
        public List<StorePack> ActivePacks { get; private set; } = new List<StorePack>();

        private const float _refreshFrequency = 1f;
        private const int _numberOfActiveOfferPacks = 3;
        private const int _offersHistoryMaxSize = _numberOfActiveOfferPacks + 1;

        private List<StorePack> _activeOfferPacks = new List<StorePack>();
        private List<StorePackData> _offerPacksDatabase = new List<StorePackData>();
        private Queue<string> _offerPacksHistory = new Queue<string>();

        private static StoreManager _instance = null;

        public static StoreManager Instance
        {
            get
            {
                ValidateSingletonInstance();
                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void ValidateSingletonInstance()
        {
            if (_instance == null)
            {
                var singletonObject = new GameObject(typeof(StoreManager).Name);
                _instance = singletonObject.AddComponent<StoreManager>();
                _instance.hideFlags = HideFlags.DontSave;
            }
        }

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