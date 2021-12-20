using JGM.GameStore.Coroutines;
using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
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
        [Inject] private IUserWallet _userWallet;
        [Inject] private ICoroutineService _coroutineService;

        private Transaction.Transaction _transaction = null;
        private IGameEventData _gameEventData;

        public void HandlePurchase(IGameEventData gameEventData)
        {
            _gameEventData = gameEventData;
            var packData = (gameEventData as PurchasePackEventData).StorePack.Data;
            _transaction = _userWallet.CreateTransaction(packData.PackCurrency, -packData.Price, packData, _coroutineService, _userWallet);
            _transaction.OnFinished.AddListener(OnTransactionFinished);
            _transaction.StartTransaction();
        }

        private void OnTransactionFinished(Transaction.Transaction transaction, bool success)
        {
            if (_transaction == transaction)
            {
                var packData = transaction.Data as PackData;
                if (success)
                {
                    if (packData.PackType == PackData.Type.Offer)
                    {
                        _eventTriggerService.Trigger("Purchase Display Offer Rewards", _gameEventData);
                    }
                    else
                    {
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
}