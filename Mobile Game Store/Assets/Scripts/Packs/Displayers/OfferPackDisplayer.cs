using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Packs.Displayers.Utils;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JGM.GameStore.Packs.Displayers
{
    public class OfferPackDisplayer : MonoBehaviour, IPackDisplayer
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _remainingTimeText;
        [SerializeField] private TextMeshProUGUI _discountText;
        [SerializeField] private TextMeshProUGUI _priceBeforeDiscountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _packItemsParent;
        [SerializeField] private GameObject _packItemPrefab;

        private TimeSpan _remainingTimeSpan = TimeSpan.Zero;

        public void SetPackData(Pack pack, IAssetsLibrary assetsLibrary)
        {
            _remainingTimeSpan = pack.RemainingTime;
            RefreshRemainingTime();
            _discountText.text = $"{pack.Data.Discount * 100}%";
            _priceBeforeDiscountText.text = pack.Data.PriceBeforeDiscount.ToString();
            _priceText.text = pack.Data.Price.ToString();
            var sortedOfferList = pack.Data.Items.OrderByDescending(o => o.ItemType);
            foreach (var item in sortedOfferList)
            {
                var packItemGO = Instantiate(_packItemPrefab);
                packItemGO.transform.SetParent(_packItemsParent, false);
                if (packItemGO.TryGetComponent<PackItemDisplayer>(out var itemToPurchase))
                {
                    itemToPurchase.IconImage.sprite = assetsLibrary.GetSprite(item.IconName);
                    if (item.ItemType == PackItemData.Type.Character)
                    {
                        var nameConverter = new CharacterNameConverter();
                        nameConverter.GetCharacterNameFromId(item.ItemId, out var characterName);
                        itemToPurchase.AmountText.text = characterName;
                    }
                    else if (item.ItemType == PackItemData.Type.Coins)
                    {
                        itemToPurchase.AmountText.text = $"{item.Amount} Coins";
                    }
                    else if (item.ItemType == PackItemData.Type.Gems)
                    {
                        itemToPurchase.AmountText.text = $"{item.Amount} Gems";
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
            _remainingTimeText.text = $"{_remainingTimeSpan.Days}d {_remainingTimeSpan.Hours}h {_remainingTimeSpan.Minutes}m {_remainingTimeSpan.Seconds}s";
        }
    }
}