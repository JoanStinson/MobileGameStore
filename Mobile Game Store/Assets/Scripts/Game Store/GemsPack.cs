using Assets.Scripts.Game_Store;
using TMPro;
using Ubisoft.UIProgrammerTest.Data;
using Ubisoft.UIProgrammerTest.Logic;
using UnityEngine;
using UnityEngine.UI;

public class GemsPack : MonoBehaviour, IPurchasePack
{
    [SerializeField] private TextMeshProUGUI _discount;
    [SerializeField] private TextMeshProUGUI _priceBeforeDiscount;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private Image _icon;
    [SerializeField] private Transform _discountBanner;

    public void OnPurchase()
    {
       
    }

    public void OnSelect()
    {
        
    }

    public void PopulatePackData(StorePack packData)
    {
        if (packData.PackData.Discount > 0)
        {
            _discount.text = $"{packData.PackData.Discount * 100}%";
        }
        else
        {
            _discountBanner.gameObject.SetActive(false);
        }
        _priceBeforeDiscount.text = packData.PackData.PriceBeforeDiscount.ToString();
        _price.text = packData.PackData.Price.ToString();
        _amount.text = packData.PackData.Items[0].Amount.ToString();
        _icon.sprite = AssetLoader.GetSprite(packData.PackData.Items[0].IconName);
    }
}
