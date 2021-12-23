using JGM.GameStore.Events.Data;
using TMPro;
using UnityEngine;

namespace JGM.GameStore.HUD
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CurrencyHUD : MonoBehaviour, IHUDElement
    {
        private TextMeshProUGUI _amountText;
        private float _amount = 0;

        private void Awake()
        {
            _amountText = GetComponent<TextMeshProUGUI>();
            _amountText.text = string.Format("{0:n0}", _amount);
        }

        public void RefreshCurrencyAmount(IGameEventData gameEventData)
        {
            RefreshCurrencyAmount(gameEventData as ICurrencyEventData);
        }

        public void RefreshCurrencyAmount(ICurrencyEventData currencyEventData)
        {
            _amount += currencyEventData.Amount;
            _amountText.text = string.Format("{0:n0}", _amount);
        }
    }
}