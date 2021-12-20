using JGM.GameStore.Coroutines;
using UnityEngine.Events;

namespace JGM.GameStore.Transaction.User
{
    public sealed partial class UserWallet : IUserWallet
    {
        public class CurrencyEvent : UnityEvent<Currency, float> { }
        public CurrencyEvent OnCurrencyChanged = new CurrencyEvent();

        private float[] _currencies = new float[(int)Currency.Count];

        public float GetCurrency(Currency currency)
        {
            return _currencies[(int)currency];
        }

        public Transaction CreateTransaction(Currency currency, float amount, object data, ICoroutineService coroutineService, IUserWallet userWallet)
        {
            var newTransaction = Transaction.Create(currency, amount, data, coroutineService, userWallet);
            return newTransaction;
        }

        public void ApplyTransaction(Transaction transaction)
        {
            if (transaction.TransactionState != Transaction.State.FinishedSuccess)
            {
                return;
            }

            _currencies[(int)transaction.TransactionCurrency] = GetCurrency(transaction.TransactionCurrency) + transaction.Amount;
            OnCurrencyChanged?.Invoke(transaction.TransactionCurrency, transaction.Amount);//TODO link this event with the HUD
        }
    }
}