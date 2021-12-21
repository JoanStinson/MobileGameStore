using JGM.GameStore.Events.Data;
using TMPro;
using UnityEngine;

namespace JGM.GameStore.HUD
{
    public class CurrencyHUD : MonoBehaviour, IHUDElement
    {
        [SerializeField]
        private TextMeshProUGUI _amountText;
        private float _amount = 0;

        private void Awake()
        {
            _amountText.text = _amount.ToString();
        }

        public void RefreshCurrencyAmount(IGameEventData gameEventData)
        {
            var data = gameEventData as RefreshCurrencyAmountEventData;
            _amount += data.Amount;
            _amountText.text = _amount.ToString();
        }
    }
}