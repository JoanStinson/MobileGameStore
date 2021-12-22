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

            var activePacks = _packsController.ActivePacks.OrderByDescending(p => p.Data.PackType)
                                                          .OrderBy(p => p.Data.Order)
                                                          .ThenBy(p => p.RemainingTime)
                                                          .ThenBy(p => p.Data.Price)
                                                          .ToArray();

            for (int i = 0; i < activePacks.Length; ++i)
            {
                var packDisplayer = _packsFactory.CreatePackDisplayer(activePacks[i]);
                _packsFactory.SetPackDisplayerParent(packDisplayer, i);
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
                var orderedPacks = packsToOrder.OrderByDescending(p => p.Data.PackType)
                                               .ThenBy(p => p.Data.Order)
                                               .ThenBy(p => p.RemainingTime)
                                               .ThenBy(p => p.Data.Price)
                                               .ToArray();

                if (orderedPacks[0] == pack)
                {
                    var previousFeaturedPack = _featuredPackSlot.Pack;
                    RemovePackDisplayer(previousFeaturedPack);

                    var newFeaturedPack = _packsFactory.CreateFeaturedOfferPack(pack);
                    _packDisplayers.Add(newFeaturedPack);
                    _featuredPackSlot = newFeaturedPack;

                    var newPack = _packsFactory.CreatePackDisplayer(previousFeaturedPack);
                    _packDisplayers.Add(newPack);

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
            OrderPacksList();

            Pack packToRemove = null;

            for (int i = 0; i < _packDisplayers.Count; ++i)
            {
                if (_featuredPackSlot == null && _packDisplayers[i].Pack.Data.Featured)
                {
                    var featuredOfferPack = _packsFactory.CreateFeaturedOfferPack(_packDisplayers[i].Pack);
                    _featuredPackSlot = featuredOfferPack;
                    _packDisplayers.Add(featuredOfferPack);
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

        private void OrderPacksList()
        {
            _packDisplayers = _packDisplayers.OrderByDescending(d => d.Pack.Data.PackType)
                                             .ThenBy(d => d.Pack.Data.Order)
                                             .ThenBy(d => d.Pack.RemainingTime)
                                             .ThenBy(d => d.Pack.Data.Price)
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
    }
}