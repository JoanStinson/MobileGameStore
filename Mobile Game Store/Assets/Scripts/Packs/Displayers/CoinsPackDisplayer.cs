using JGM.GameStore.Loaders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JGM.GameStore.Packs.Displayers
{
    public class CoinsPackDisplayer : MonoBehaviour, IPackDisplayer
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TextMeshProUGUI _discountText;
        [SerializeField] private TextMeshProUGUI _priceBeforeDiscountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _discountParentTransform;

        public void SetPackData(Pack pack, IAssetsLibrary assetsLibrary)
        {
            if (pack.Data.Discount > 0)
            {
                _discountText.text = $"{pack.Data.Discount * 100}%";
            }
            else
            {
                _discountParentTransform.gameObject.SetActive(false);
            }
            _priceBeforeDiscountText.text = pack.Data.PriceBeforeDiscount.ToString();
            _priceText.text = pack.Data.Price.ToString();
            _amountText.text = $"{pack.Data.Items[0].Amount} Coins";
            _iconImage.sprite = assetsLibrary.GetSprite(pack.Data.Items[0].IconName);
        }
    }
}