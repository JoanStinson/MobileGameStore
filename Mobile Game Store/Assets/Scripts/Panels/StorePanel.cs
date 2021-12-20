using JGM.GameStore.Loaders;
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
        [Header("Settings")]
        [SerializeField] private uint _storeRefreshFrequencyInSeconds;

        [Header("Prefabs")]
        [SerializeField] private GameObject _offerPackPrefab;
        [SerializeField] private GameObject _gemsPackPrefab;
        [SerializeField] private GameObject _coinsPackPrefab;
        [SerializeField] private GameObject _featuredPackPrefab;

        [Header("Parents")]
        [SerializeField] private Transform _offerPacksParent;
        [SerializeField] private Transform _gemsPacksParent;
        [SerializeField] private Transform _coinsPacksParent;
        [SerializeField] private Transform _featuredPacksParent;

        [Inject]
        private IAssetsLibrary _storeAssetsLibrary;
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
                        InstantiateAndSetPackDataInGUI(pack, _featuredPackPrefab, _featuredPacksParent);
                    }
                }
                else
                {
                    switch (pack.Data.PackType)
                    {
                        case PackData.Type.Offer:
                            InstantiateAndSetPackDataInGUI(pack, _offerPackPrefab, _offerPacksParent);
                            break;

                        case PackData.Type.Gems:
                            InstantiateAndSetPackDataInGUI(pack, _gemsPackPrefab, _gemsPacksParent);
                            break;

                        case PackData.Type.Coins:
                            InstantiateAndSetPackDataInGUI(pack, _coinsPackPrefab, _coinsPacksParent);
                            break;
                    }
                }
            }
        }

        private void InstantiateAndSetPackDataInGUI(Pack pack, GameObject prefab, Transform parent)
        {
            var spawnedGO = Instantiate(prefab);
            spawnedGO.transform.SetParent(parent, false);
            _storePacksGUIObjects.Add(spawnedGO);
            if (spawnedGO.TryGetComponent<IPackDisplayer>(out var storePackDisplayer))
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