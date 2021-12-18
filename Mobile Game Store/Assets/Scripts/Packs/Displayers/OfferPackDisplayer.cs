using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Data;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JGM.GameStore.Packs.Displayers
{
    public class OfferPackDisplayer : MonoBehaviour, IStorePackDisplayer
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _remainingTime;
        [SerializeField] private TextMeshProUGUI _discount;
        [SerializeField] private TextMeshProUGUI _priceBeforeDiscount;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Transform _packItemsParent;
        [SerializeField] private GameObject _packItemPrefab;

        private TimeSpan _remainingTimeSpan = TimeSpan.Zero;

        public void SetPackData(StorePack storePack, IStoreAssetsLibrary assetsLibrary)
        {
            _remainingTimeSpan = storePack.RemainingTime;
            RefreshRemainingTime();
            _discount.text = $"{storePack.PackData.Discount * 100}%";
            _priceBeforeDiscount.text = storePack.PackData.PriceBeforeDiscount.ToString();
            _price.text = storePack.PackData.Price.ToString();
            var sortedOfferList = storePack.PackData.Items.OrderByDescending(o => o.ItemType);
            foreach (var item in sortedOfferList)
            {
                var packItemGO = Instantiate(_packItemPrefab);
                packItemGO.transform.SetParent(_packItemsParent, false);
                if (packItemGO.TryGetComponent<StoreItemDisplayerData>(out var itemToPurchase))
                {
                    itemToPurchase.Icon.sprite = assetsLibrary.GetSprite(item.IconName);
                    if (item.ItemType == StoreItemData.Type.Character)
                    {
                        var nameConverter = new CharacterNameConverter();
                        nameConverter.GetCharacterNameFromId(item.ItemId, out var characterName);
                        itemToPurchase.Amount.text = characterName;
                    }
                    else if (item.ItemType == StoreItemData.Type.Coins)
                    {
                        itemToPurchase.Amount.text = $"{item.Amount} Coins";
                    }
                    else if (item.ItemType == StoreItemData.Type.Gems)
                    {
                        itemToPurchase.Amount.text = $"{item.Amount} Gems";
                    }
                }
            }
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

        private void RefreshRemainingTime()
        {
            _remainingTime.text = $"{_remainingTimeSpan.Days}d {_remainingTimeSpan.Hours}h {_remainingTimeSpan.Minutes}m {_remainingTimeSpan.Seconds}s";
        }
    }
}