using JGM.GameStore.Packs.Data;
using JGM.GameStore.Packs.Displayers;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Packs
{
    public class PacksFactory : MonoBehaviour, IPacksFactory
    {
        [SerializeField] private Transform _featuredOfferPacksParent;
        [SerializeField] private Transform _offerPacksParent;
        [SerializeField] private Transform _gemsPacksParent;
        [SerializeField] private Transform _coinsPacksParent;

        [Inject] private OfferPackDisplayer.FeaturedFactory _featuredOfferPackFactory;
        [Inject] private OfferPackDisplayer.Factory _offersPackFactory;
        [Inject] private GemsPackDisplayer.Factory _gemsPackFactory;
        [Inject] private CoinsPackDisplayer.Factory _coinsPackFactory;

        private int _gemsSiblingIndexOffset;
        private int _coinsSiblingIndexOffset;

        public PackDisplayer CreatePackDisplayer(in Pack pack)
        {
            PackDisplayer packDisplayer = null;

            switch (pack.Data.PackType)
            {
                case PackData.Type.Offer:
                    {
                        packDisplayer = _offersPackFactory.Create();
                    }
                    break;

                case PackData.Type.Gems:
                    {
                        packDisplayer = _gemsPackFactory.Create();
                    }
                    break;

                case PackData.Type.Coins:
                    {
                        packDisplayer = _coinsPackFactory.Create();
                    }
                    break;
            }

            packDisplayer?.SetPackData(pack);
            return packDisplayer;
        }

        public OfferPackDisplayer CreateFeaturedOfferPack(in Pack pack)
        {
            var featuredOfferPackDisplayer = _featuredOfferPackFactory.Create();
            featuredOfferPackDisplayer.SetPackData(pack);
            featuredOfferPackDisplayer.transform.SetParent(_featuredOfferPacksParent, false);
            return featuredOfferPackDisplayer;
        }

        public void SetPackDisplayerParent(PackDisplayer packDisplayer, int index)
        {
            switch (packDisplayer.Pack.Data.PackType)
            {
                case PackData.Type.Offer:
                    {
                        packDisplayer.transform.SetParent(_offerPacksParent, false);
                        packDisplayer.transform.SetSiblingIndex(index);
                    }
                    break;

                case PackData.Type.Gems:
                    {
                        if (_gemsSiblingIndexOffset == 0)
                        {
                            _gemsSiblingIndexOffset = index;
                        }
                        packDisplayer.transform.SetParent(_gemsPacksParent, false);
                        packDisplayer.transform.SetSiblingIndex(index - _gemsSiblingIndexOffset);
                    }
                    break;

                case PackData.Type.Coins:
                    {
                        if (_coinsSiblingIndexOffset == 0)
                        {
                            _coinsSiblingIndexOffset = index;
                        }
                        packDisplayer.transform.SetParent(_coinsPacksParent, false);
                        packDisplayer.transform.SetSiblingIndex(index - _coinsSiblingIndexOffset);
                    }
                    break;
            }
        }

        public void ResetSiblingIndexes()
        {
            _gemsSiblingIndexOffset = 0;
            _coinsSiblingIndexOffset = 0;
        }
    }
}