using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Data;
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

        public void SetPackData(StorePack storePack)
        {
            _remainingTime.text = $"{storePack.RemainingTime.Days}d {storePack.RemainingTime.Hours}h {storePack.RemainingTime.Minutes}m {storePack.RemainingTime.Seconds}s";
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
                    itemToPurchase.Icon.sprite = AssetLoader.GetSprite(item.IconName);
                    if (item.ItemType == StoreItemData.Type.Character)
                    {
                        itemToPurchase.Amount.text = $"{item.Amount} Dragons";
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
    }
}