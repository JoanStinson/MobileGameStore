using JGM.GameStore.Loaders;
using JGM.GameStore.Packs;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Packs.Displayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace JGM.GameStore
{
    public sealed class StoreManager : MonoBehaviour
    {
        [SerializeField] private GameObject _offerPackPrefab;
        [SerializeField] private GameObject _gemsPackPrefab;
        [SerializeField] private GameObject _coinsPackPrefab;
        [SerializeField] private GameObject _featuredPackPrefab;
        [Space]
        [SerializeField] private Transform _offerPacksParent;
        [SerializeField] private Transform _gemsPacksParent;
        [SerializeField] private Transform _coinsPacksParent;
        [SerializeField] private Transform _featuredPacksParent;
        [Space]
        [SerializeField] private uint _storeRefreshFrequencyInSeconds;

        private IStorePacksController _storePacksController;
        private IStoreAssetsLibrary _storeAssetsLibrary;
        private List<GameObject> _storePacksGUIObjects;

        private void Awake()
        {
            _storePacksController = new StorePacksController();
            _storePacksController.Initialize();
            _storePacksController.Refresh();
            _storeAssetsLibrary = new StoreAssetsLibrary();
            _storeAssetsLibrary.Initialize();
            _storePacksGUIObjects = new List<GameObject>();
        }

        private void Start()
        {
            RefreshStoreGUI();
        }

        private async void Update()
        {
            await Task.Delay(TimeSpan.FromSeconds(_storeRefreshFrequencyInSeconds));
            _storePacksController.Refresh();
            //foreach (var go in _storePacksGUIObjects)
            //{
            //    Destroy(go);
            //}
            //_storePacksGUIObjects.Clear();
            //RefreshStoreGUI();
        }

        private void RefreshStoreGUI()
        {
            var sortedPacksList = _storePacksController.ActivePacks
                                                       .OrderByDescending(p => p.PackData.PackType)
                                                       .ThenBy(p => p.PackData.Order)
                                                       .ThenBy(p => p.RemainingTime)
                                                       .ThenBy(p => p.PackData.Price);

            bool isFeaturedSlotOccupied = false;

            foreach (var pack in sortedPacksList)
            {
                if (!isFeaturedSlotOccupied)
                {
                    bool canPackBeFeatured = (pack.PackData.PackType == StorePackData.Type.Offer && pack.PackData.Featured);
                    if (canPackBeFeatured)
                    {
                        isFeaturedSlotOccupied = true;
                        InstantiateAndSetPackDataInGUI(pack, _featuredPackPrefab, _featuredPacksParent);
                    }
                }
                else
                {
                    switch (pack.PackData.PackType)
                    {
                        case StorePackData.Type.Offer:
                            InstantiateAndSetPackDataInGUI(pack, _offerPackPrefab, _offerPacksParent);
                            break;

                        case StorePackData.Type.Gems:
                            InstantiateAndSetPackDataInGUI(pack, _gemsPackPrefab, _gemsPacksParent);
                            break;

                        case StorePackData.Type.Coins:
                            InstantiateAndSetPackDataInGUI(pack, _coinsPackPrefab, _coinsPacksParent);
                            break;
                    }
                }
            }
        }

        private void InstantiateAndSetPackDataInGUI(StorePack pack, GameObject prefab, Transform parent)
        {
            var spawnedGO = Instantiate(prefab);
            spawnedGO.transform.SetParent(parent, false);
            _storePacksGUIObjects.Add(spawnedGO);
            if (spawnedGO.TryGetComponent<IStorePackDisplayer>(out var storePackDisplayer))
            {
                storePackDisplayer.SetPackData(pack, _storeAssetsLibrary);
            }
            else
            {
                throw new MissingComponentException($"Missing {nameof(storePackDisplayer)}");
            }
        }
    }
}