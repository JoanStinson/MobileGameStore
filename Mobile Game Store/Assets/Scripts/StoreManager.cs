using JGM.GameStore.Packs;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Packs.Displayers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField]
        private StorePacksController _storeController;

        private List<StorePack> _packs;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);

            _packs = new List<StorePack>();

            foreach (StorePack pack in _storeController.ActivePacks)
            {
                _packs.Add(pack);
            }

            var sortedOfferList = _packs.OrderByDescending(o => o.PackData.PackType).ThenBy(o => o.PackData.Order).ThenBy(o => o.RemainingTime).ThenBy(o => o.PackData.Price);

            bool featuredPackOccupied = false;
            foreach (var pack in sortedOfferList)
            {
                if (!featuredPackOccupied)
                {
                    if (pack.PackData.PackType == StorePackData.Type.Offer && pack.PackData.Featured)
                    {
                        featuredPackOccupied = true;
                        var offerGO = Instantiate(_featuredPackPrefab);
                        offerGO.transform.SetParent(_featuredPacksParent, false);
                        if (offerGO.TryGetComponent<IStorePackDisplayer>(out var offerPack))
                        {
                            offerPack.SetPackData(pack);
                        }
                        else
                        {
                            throw new MissingComponentException("Missing Offer Pack Component");
                        }
                    }
                }
                else
                {
                    GameObject prefab = null;
                    Transform parent = null;
                    if (pack.PackData.PackType == StorePackData.Type.Gems)
                    {
                        prefab = _gemsPackPrefab;
                        parent = _gemsPacksParent;
                    }
                    else if (pack.PackData.PackType == StorePackData.Type.Coins)
                    {
                        prefab = _coinsPackPrefab;
                        parent = _coinsPacksParent;
                    }
                    else if (pack.PackData.PackType == StorePackData.Type.Offer)
                    {
                        prefab = _offerPackPrefab;
                        parent = _offerPacksParent;
                    }

                    var offerGO = Instantiate(prefab);
                    offerGO.transform.SetParent(parent, false);
                    if (offerGO.TryGetComponent<IStorePackDisplayer>(out var offerPack))
                    {
                        offerPack.SetPackData(pack);
                    }
                    else
                    {
                        throw new MissingComponentException("Missing Offer Pack Component");
                    }
                }
            }
        }
    }
}