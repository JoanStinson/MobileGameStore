using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace JGM.GameStore.Transaction.User
{
    public sealed partial class UserProfileService : MonoBehaviour, IUserProfileService
    {
        public class CurrencyEvent : UnityEvent<Currency, float> { }
        [HideInInspector]
        public CurrencyEvent OnCurrencyChanged = new CurrencyEvent();

        [Inject]
        private Transaction.Factory _transactionFactory;
        private float[] _currencies = new float[(int)Currency.Count];

        public Transaction CreateTransaction(Currency currency, float amount, object data = null)
        {
            var newTransaction = _transactionFactory.Create();
            newTransaction.SetData(currency, amount, data);
            return newTransaction;
        }

        public void ApplyTransaction(in Transaction transaction)
        {
            if (transaction.TransactionState != Transaction.State.FinishedSuccess)
            {
                return;
            }

            _currencies[(int)transaction.TransactionCurrency] = GetCurrency(transaction.TransactionCurrency) + transaction.Amount;
            OnCurrencyChanged?.Invoke(transaction.TransactionCurrency, transaction.Amount);
        }

        public float GetCurrency(Currency currency)
        {
            return _currencies[(int)currency];
        }
    }
}