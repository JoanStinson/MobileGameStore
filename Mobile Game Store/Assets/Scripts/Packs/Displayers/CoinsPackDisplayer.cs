using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Libraries;
using JGM.GameStore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Packs.Displayers
{
    public class CoinsPackDisplayer : MonoBehaviour, IPackDisplayer
    {
        public class Factory : PlaceholderFactory<CoinsPackDisplayer> { }

        [SerializeField] private Image _iconImage;
        [SerializeField] private LocalizedText _amountText;
        [SerializeField] private TextMeshProUGUI _discountText;
        [SerializeField] private TextMeshProUGUI _priceBeforeDiscountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _discountParentTransform;

        [Inject]
        private IEventTriggerService _eventTriggerService;
        private Pack _pack;

        public void SetPackData(in Pack pack, IAssetsLibrary assetsLibrary)
        {
            _pack = pack;
            _priceBeforeDiscountText.text = pack.Data.PriceBeforeDiscount.ToString();
            _priceText.text = pack.Data.Price.ToString();
            _iconImage.sprite = assetsLibrary.GetSprite(pack.Data.Items[0].IconName);
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

        public void PurchasePack()
        {
            var eventData = new PurchasePackEventData(_pack);
            _eventTriggerService.Trigger("Purchase Pack", eventData);
        }
    }
}