using JGM.GameStore.Libraries;
using JGM.GameStore.Packs;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Packs.Displayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Panels
{
    [RequireComponent(typeof(IPacksController))]
    public sealed class StorePanel : MonoBehaviour
    {
        [SerializeField] private uint _storeRefreshFrequencyInSeconds;
        [Space]
        [SerializeField] private Transform _featuredPacksParent;
        [SerializeField] private Transform _offerPacksParent;
        [SerializeField] private Transform _gemsPacksParent;
        [SerializeField] private Transform _coinsPacksParent;

        [Inject] private OfferPackDisplayer.FeaturedFactory _featuredOffersPackFactory;
        [Inject] private OfferPackDisplayer.Factory _offersPackFactory;
        [Inject] private GemsPackDisplayer.Factory _gemsPackFactory;
        [Inject] private CoinsPackDisplayer.Factory _coinsPackFactory;
        [Inject] private IAssetsLibrary _storeAssetsLibrary;

        private IPacksController _storePacksController;
        private List<GameObject> _storePacksGUIObjects;

        private void Awake()
        {
            _storePacksGUIObjects = new List<GameObject>();
            _storePacksController = GetComponent<IPacksController>();
            _storePacksController.Initialize();
            _storePacksController.Refresh();
        }

        private void Start()
        {
            RefreshStoreGUI();
        }

        private async void Update()
        {
            await Task.Delay(TimeSpan.FromSeconds(_storeRefreshFrequencyInSeconds));
            _storePacksController.Refresh();
        }

        private void RefreshStoreGUI()
        {
            var sortedPacksList = _storePacksController.ActivePacks
                                                       .OrderByDescending(p => p.Data.PackType)
                                                       .ThenBy(p => p.Data.Order)
                                                       .ThenBy(p => p.RemainingTime)
                                                       .ThenBy(p => p.Data.Price);

            bool isFeaturedSlotOccupied = false;

            foreach (var pack in sortedPacksList)
            {
                if (!isFeaturedSlotOccupied)
                {
                    bool canPackBeFeatured = (pack.Data.PackType == PackData.Type.Offer && pack.Data.Featured);
                    if (canPackBeFeatured)
                    {
                        isFeaturedSlotOccupied = true;
                        var spawnedPack = _featuredOffersPackFactory.Create();
                        spawnedPack.transform.SetParent(_featuredPacksParent, false);
                        _storePacksGUIObjects.Add(spawnedPack.gameObject);
                        spawnedPack.SetPackData(pack, _storeAssetsLibrary);
                    }
                }
                else
                {
                    switch (pack.Data.PackType)
                    {
                        case PackData.Type.Offer:
                            {
                                var spawnedPack = _offersPackFactory.Create();
                                spawnedPack.transform.SetParent(_offerPacksParent, false);
                                _storePacksGUIObjects.Add(spawnedPack.gameObject);
                                spawnedPack.SetPackData(pack, _storeAssetsLibrary);
                            }
                            break;

                        case PackData.Type.Gems:
                            {
                                var spawnedPack = _gemsPackFactory.Create();
                                spawnedPack.transform.SetParent(_gemsPacksParent, false);
                                _storePacksGUIObjects.Add(spawnedPack.gameObject);
                                spawnedPack.SetPackData(pack, _storeAssetsLibrary);
                            }
                            break;

                        case PackData.Type.Coins:
                            {
                                var spawnedPack = _coinsPackFactory.Create();
                                spawnedPack.transform.SetParent(_coinsPacksParent, false);
                                _storePacksGUIObjects.Add(spawnedPack.gameObject);
                                spawnedPack.SetPackData(pack, _storeAssetsLibrary);
                            }
                            break;
                    }
                }
            }
        }
    }
}