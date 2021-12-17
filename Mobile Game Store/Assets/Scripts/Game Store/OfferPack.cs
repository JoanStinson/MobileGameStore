using Assets.Scripts.Game_Store;
using TMPro;
using Ubisoft.UIProgrammerTest.Data;
using Ubisoft.UIProgrammerTest.Logic;
using UnityEngine;

public class OfferPack : MonoBehaviour, IPurchasePack
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _remainingTime;
    [SerializeField] private TextMeshProUGUI _discount;
    [SerializeField] private TextMeshProUGUI _priceBeforeDiscount;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Transform _packItemsParent;
    [SerializeField] private GameObject _packItemPrefab;

    public void OnPurchase()
    {
        
    }

    public void OnSelect()
    {
        
    }

    public void PopulatePackData(StorePack packData)
    {
        _remainingTime.text = packData.RemainingTime.ToString();
        _discount.text = packData.PackData.Discount.ToString();
        _priceBeforeDiscount.text = packData.PackData.PriceBeforeDiscount.ToString();
        _price.text = packData.PackData.Price.ToString();
        foreach(var item in packData.PackData.Items)
        {
            var packItemGO = Instantiate(_packItemPrefab);
            packItemGO.transform.SetParent(_packItemsParent, false);
            if (packItemGO.TryGetComponent<PackItemToPurchase>(out var itemToPurchase))
            {
                itemToPurchase._icon.sprite = AssetLoader.GetSprite(item.IconName);
                itemToPurchase._amount.text = item.Amount.ToString();
            }
        }
    }
}
