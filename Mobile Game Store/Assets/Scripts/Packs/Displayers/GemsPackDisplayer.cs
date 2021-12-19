using JGM.GameStore.Events;
using JGM.GameStore.Events.Data;
using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JGM.GameStore.Packs.Displayers
{
    public class GemsPackDisplayer : MonoBehaviour, IPackDisplayer
    {
        [SerializeField] private TextMeshProUGUI _discount;
        [SerializeField] private TextMeshProUGUI _priceBeforeDiscount;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private Image _icon;
        [SerializeField] private Transform _discountBanner;
        [SerializeField] private GameEvent _gameEvent;

        private Pack _storePack;

        public void SetPackData(Pack storePack, IAssetsLibrary assetsLibrary)
        {
            if (storePack.PackData.Discount > 0)
            {
                _discount.text = $"{storePack.PackData.Discount * 100}%";
            }
            else
            {
                _discountBanner.gameObject.SetActive(false);
            }
            _priceBeforeDiscount.text = storePack.PackData.PriceBeforeDiscount.ToString();
            _price.text = storePack.PackData.Price.ToString();
            _amount.text = $"{storePack.PackData.Items[0].Amount} Gems";
            _icon.sprite = assetsLibrary.GetSprite(storePack.PackData.Items[0].IconName);
            _storePack = storePack;
        }

        public void DisableAndSendPurchasePackEvent()
        {
            _gameEvent.Trigger(new PurchasePackEventData(_storePack.PackData.Items, _storePack.PackData.PackCurrency, _storePack.PackData.Price));
        }
    }
}