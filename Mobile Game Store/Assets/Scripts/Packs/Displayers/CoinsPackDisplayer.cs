using JGM.GameStore.Events;
using JGM.GameStore.Events.Data;
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
        [SerializeField] private GameEvent _gameEvent;

        private Pack _storePack;

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
            _storePack = pack;
        }

        public void PurchasePack()
        {
            var eventData = new PurchasePackEventData(_storePack.Data.Items, _storePack.Data.PackCurrency, _storePack.Data.Price, _storePack.Data.PackType);
            _gameEvent.Trigger(eventData);
        }
    }
}