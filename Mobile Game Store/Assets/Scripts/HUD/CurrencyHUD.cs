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
            _amountText.text = string.Format("{0:n0}", _amount);
        }

        public void RefreshCurrencyAmount(IGameEventData gameEventData)
        {
            var data = gameEventData as RefreshCurrencyAmountEventData;
            _amount += data.Amount;
            _amountText.text = string.Format("{0:n0}", _amount);
        }
    }
}