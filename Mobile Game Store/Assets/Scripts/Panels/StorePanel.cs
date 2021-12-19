using JGM.GameStore.Loaders;
using JGM.GameStore.Packs;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Packs.Displayers;
using JGM.GameStore.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Panels
{
    public sealed class StorePanel : MonoBehaviour
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
        [SerializeField] private PackItem3DVisualizer _rewardsPreviewer;

        private IPacksController _storePacksController;
        [Inject]
        private IAssetsLibrary _storeAssetsLibrary;
        private List<GameObject> _storePacksGUIObjects;

        private void Awake()
        {
            _storePacksController = new PacksController();
            _storePacksController.Initialize();
            _storePacksController.Refresh();
            //_storeAssetsLibrary = new StoreAssetsLibrary();
            //_storeAssetsLibrary.Initialize();
            _storePacksGUIObjects = new List<GameObject>();
        }

        private void Start()
        {
            RefreshStoreGUI();
            //GameObject.FindGameObjectWithTag("Respawn").GetComponent<RawImage>().texture = _rewardsPreviewer.GetRenderTexture("PF_Character1");
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
                            {
                                InstantiateAndSetPackDataInGUI(pack, _offerPackPrefab, _offerPacksParent);
                                for (int i = 0; i < pack.Data.Items.Length; ++i)
                                {
                                    _rewardsPreviewer.Store3DPreview(pack.Data.Items[0].PrefabName);
                                }
                            }
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