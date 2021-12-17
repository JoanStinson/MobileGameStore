using Ubisoft.UIProgrammerTest.Logic;
using UnityEngine;
using UnityEngine.Events;

namespace Ubisoft.UIProgrammerTest.Singletons
{
    public class UserProfile
    {
        public enum Currency
        {
            Coins,
            Gems,
            Dollars,
            Count
        }

        public class CurrencyEvent : UnityEvent<Currency, float> { }
        public CurrencyEvent OnCurrencyChanged = new CurrencyEvent();

        private float[] _currencies = new float[(int)Currency.Count];

        private static UserProfile _instance = null;

        public static UserProfile Instance
        {
            get
            {
                ValidateSingletonInstance();
                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void ValidateSingletonInstance()
        {
            if (_instance == null)
            {
                _instance = new UserProfile();
            }
        }

        public float GetCurrency(Currency currency)
        {
            return _currencies[(int)currency];
        }

        public Transaction CreateTransaction(Currency currency, float amount)
        {
            var newTransaction = Transaction.Create(currency, amount);
            return newTransaction;
        }

        public void ApplyTransaction(Transaction transaction)
        {
            if (transaction.TransactionState != Transaction.State.FinishedSuccess)
            {
                return;
            }

            _currencies[(int)transaction.TransactionCurrency] = GetCurrency(transaction.TransactionCurrency) + transaction.Amount;
            OnCurrencyChanged?.Invoke(transaction.TransactionCurrency, transaction.Amount);
        }
    }
}