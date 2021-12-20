using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Packs;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Transaction.User;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private Transform _loadingWindow;

        [Inject] private IEventTriggerService _eventTriggerService;
        [Inject] private IUserProfileService _userWallet;

        private Transaction.Transaction _transaction = null;
        private IGameEventData _gameEventData;

        public void HandlePurchase(IGameEventData gameEventData)
        {
            _gameEventData = gameEventData;
            var pack = (gameEventData as PurchasePackEventData).StorePack;
            _transaction = _userWallet.CreateTransaction(pack.Data.PackCurrency, -pack.Data.Price, pack);
            _transaction.OnFinished.AddListener(OnTransactionFinished);
            _transaction.StartTransaction();
        }

        private void OnTransactionFinished(Transaction.Transaction transaction, bool success)
        {
            if (_transaction != transaction)
            {
                return;
            }

            if (success)
            {
                var pack = transaction.Data as Pack;
                pack.ApplyTransaction();

                if (pack.Data.PackType == PackData.Type.Offer)
                {
                    _eventTriggerService.Trigger("Purchase Display Offer Rewards", _gameEventData);
                }
                else
                {
                    if (pack.Data.PackType == PackData.Type.Coins)
                    {
                        var eventData = new RefreshCurrencyAmountEventData(transaction.Amount);
                        _eventTriggerService.Trigger("Refresh Gems Amount", eventData);
                    }
                    _eventTriggerService.Trigger("Purchase Display Currency Rewards", _gameEventData);
                }
            }
            else
            {
                _eventTriggerService.Trigger("Purchase Error");
            }

            _loadingWindow.gameObject.SetActive(false);
            _transaction = null;
        }
    }
}