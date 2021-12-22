using JGM.GameStore.Packs;
using JGM.GameStore.Packs.Displayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace JGM.GameStore.Panels
{
    [RequireComponent(typeof(IPacksController))]
    [RequireComponent(typeof(IPacksFactory))]
    public sealed class StorePanel : MonoBehaviour
    {
        [SerializeField]
        private uint _storeRefreshFrequencyInSeconds;

        private IPacksController _packsController;
        private IPacksFactory _packsFactory;
        private List<PackDisplayer> _packDisplayers;
        private OfferPackDisplayer _featuredPackSlot = null;

        private void Awake()
        {
            _packDisplayers = new List<PackDisplayer>();
            _packsController = GetComponent<IPacksController>();
            _packsFactory = GetComponent<IPacksFactory>();
        }

        private void Start()
        {
            _packsController.Initialize();
            _packsController.Refresh();
            _packsController.OnPackActivated.AddListener(OnPackActivated);
            _packsController.OnPackRemoved.AddListener(OnPackRemoved);

            GetOrderedPacksList(_packsController.ActivePacks, out var activePacks);
            for (int i = 0; i < activePacks.Count; ++i)
            {
                var packDisplayer = _packsFactory.CreatePackDisplayer(activePacks[i]);
                _packDisplayers.Add(packDisplayer);
            }

            RefrehStoreGUI();
        }

        private async void Update()
        {
            await Task.Delay(TimeSpan.FromSeconds(_storeRefreshFrequencyInSeconds));
            _packsController.Refresh();
        }

        private void OnPackActivated(Pack pack)
        {
            if (_featuredPackSlot != null)
            {
                var packsToOrder = new List<Pack>() { _featuredPackSlot.Pack, pack };
                GetOrderedPacksList(packsToOrder, out var orderedPacks);

                if (orderedPacks[0] == pack)
                {
                    SwapFeaturedPack(pack);
                    RefrehStoreGUI();
                    return;
                }
            }

            var newPackDisplayer = _packsFactory.CreatePackDisplayer(pack);
            _packDisplayers.Add(newPackDisplayer);
            RefrehStoreGUI();
        }

        private void OnPackRemoved(Pack pack)
        {
            RemovePackDisplayer(pack);

            if (_featuredPackSlot.Pack == pack)
            {
                _featuredPackSlot = null;
            }

            RefrehStoreGUI();
        }

        private void RefrehStoreGUI()
        {
            OrderPackDisplayersList();

            Pack packToRemove = null;

            for (int i = 0; i < _packDisplayers.Count; ++i)
            {
                if (_featuredPackSlot == null && _packDisplayers[i].Pack.Data.Featured)
                {
                    CreateNewFeaturedPack(_packDisplayers[i].Pack);
                    packToRemove = _packDisplayers[i].Pack;
                }
                else if (_featuredPackSlot != _packDisplayers[i])
                {
                    _packsFactory.SetPackDisplayerParent(_packDisplayers[i], i);
                }
            }

            if (packToRemove != null)
            {
                RemovePackDisplayer(packToRemove);
            }

            _packsFactory.ResetSiblingIndexes();
        }

        private void OrderPackDisplayersList()
        {
            _packDisplayers = _packDisplayers.OrderByDescending(d => d.Pack.Data.PackType)
                                             .ThenBy(d => d.Pack.Data.Order)
                                             .ThenBy(d => d.Pack.RemainingTime)
                                             .ThenBy(d => d.Pack.Data.Price)
                                             .ToList();
        }

        private void GetOrderedPacksList(in List<Pack> unorderedList, out List<Pack> orderedList)
        {
            orderedList = unorderedList.OrderByDescending(p => p.Data.PackType)
                                       .ThenBy(p => p.Data.Order)
                                       .ThenBy(p => p.RemainingTime)
                                       .ThenBy(p => p.Data.Price)
                                       .ToList();
        }

        private void RemovePackDisplayer(Pack pack)
        {
            PackDisplayer packDisplayerToRemove = null;

            foreach (var packDisplayer in _packDisplayers)
            {
                if (packDisplayer.Pack == pack)
                {
                    packDisplayerToRemove = packDisplayer;
                    break;
                }
            }

            if (packDisplayerToRemove != null)
            {
                _packDisplayers.Remove(packDisplayerToRemove);
                Destroy(packDisplayerToRemove.gameObject);
            }
        }

        private void SwapFeaturedPack(Pack pack)
        {
            var previousFeaturedPack = _featuredPackSlot.Pack;
            RemovePackDisplayer(previousFeaturedPack);

            CreateNewFeaturedPack(pack);

            var newPack = _packsFactory.CreatePackDisplayer(previousFeaturedPack);
            _packDisplayers.Add(newPack);
        }

        private void CreateNewFeaturedPack(Pack pack)
        {
            var newFeaturedPack = _packsFactory.CreateFeaturedOfferPack(pack);
            _packDisplayers.Add(newFeaturedPack);
            _featuredPackSlot = newFeaturedPack;
        }
    }
}