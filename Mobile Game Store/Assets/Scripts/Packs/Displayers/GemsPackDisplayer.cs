using JGM.GameStore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Packs.Displayers
{
    public class GemsPackDisplayer : PackDisplayer
    {
        public class Factory : PlaceholderFactory<GemsPackDisplayer> { }

        [SerializeField] private Image _iconImage;
        [SerializeField] private LocalizedText _amountText;
        [SerializeField] private TextMeshProUGUI _discountText;
        [SerializeField] private TextMeshProUGUI _priceBeforeDiscountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _discountParentTransform;

        public override void SetPackData(in Pack pack)
        {
            base.SetPackData(pack);

            _priceBeforeDiscountText.text = $"{pack.Data.PriceBeforeDiscount}$";
            _priceText.text = $"{pack.Data.Price}$";
            _iconImage.sprite = _assetsLibrary.GetSprite(pack.Data.Items[0].IconName);
            _amountText.RefreshText(pack.Data.Items[0].TextId, $"{string.Format("{0:n0}", pack.Data.Items[0].Amount)} ");

            if (pack.Data.Discount > 0)
            {
                _discountText.text = $"{pack.Data.Discount * 100}%";
            }
            else
            {
                _discountParentTransform.gameObject.SetActive(false);
            }
        }
    }
}