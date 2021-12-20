using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Loaders;
using JGM.GameStore.Localization;
using JGM.GameStore.Packs.Data;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Packs.Displayers
{
    public class OfferPackDisplayer : MonoBehaviour, IPackDisplayer
    {
        public class Factory : PlaceholderFactory<OfferPackDisplayer> { }
        public class FeaturedFactory : PlaceholderFactory<OfferPackDisplayer> { }

        [SerializeField] private LocalizedText _titleText;
        [SerializeField] private TextMeshProUGUI _remainingTimeText;
        [SerializeField] private TextMeshProUGUI _discountText;
        [SerializeField] private TextMeshProUGUI _priceBeforeDiscountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _packItemsParent;

        [Inject] private IEventTriggerService _eventTriggerService;
        [Inject] private PackItemDisplayer.Factory _packItemDisplayerFactory;
        private TimeSpan _remainingTimeSpan = TimeSpan.Zero;
        private Pack _pack;

        public void SetPackData(in Pack pack, IAssetsLibrary assetsLibrary)
        {
            _pack = pack;
            _remainingTimeSpan = pack.RemainingTime;
            RefreshRemainingTime();
            _discountText.text = $"{pack.Data.Discount * 100}%";
            _priceBeforeDiscountText.text = pack.Data.PriceBeforeDiscount.ToString();
            _priceText.text = pack.Data.Price.ToString();
            SetPackItems(pack, assetsLibrary);
            _titleText.RefreshText(pack.Data.TextId);
        }

        public void PurchasePack()
        {
            var eventData = new PurchasePackEventData(_pack);
            _eventTriggerService.Trigger("Purchase Pack", eventData);
        }

        private void Update()
        {
            if (_remainingTimeSpan > TimeSpan.Zero)
            {
                var deltaTimeSpan = TimeSpan.FromSeconds(Time.deltaTime);
                _remainingTimeSpan = _remainingTimeSpan.Subtract(deltaTimeSpan);
                RefreshRemainingTime();
            }
        }

        private void SetPackItems(Pack pack, IAssetsLibrary assetsLibrary)
        {
            var sortedOfferList = pack.Data.Items.OrderByDescending(o => o.ItemType);
            foreach (var item in sortedOfferList)
            {
                var spawnedPackItem = _packItemDisplayerFactory.Create();
                spawnedPackItem.transform.SetParent(_packItemsParent, false);
                if (spawnedPackItem.TryGetComponent<PackItemDisplayer>(out var packItemDisplayer))
                {
                    packItemDisplayer.IconImage.sprite = assetsLibrary.GetSprite(item.IconName);
                    if (item.ItemType == PackItemData.Type.Character)
                    {
                        packItemDisplayer.AmountText.RefreshText(item.TextId);
                    }
                    else
                    {
                        packItemDisplayer.AmountText.RefreshText(item.TextId, $"{item.Amount} ");
                    }
                }
            }
        }

        private void RefreshRemainingTime()
        {
            _remainingTimeText.text = $"{_remainingTimeSpan.Days}d {_remainingTimeSpan.Hours}h {_remainingTimeSpan.Minutes}m {_remainingTimeSpan.Seconds}s";
        }
    }
}