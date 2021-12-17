using Ubisoft.UIProgrammerTest.Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace Ubisoft.UIProgrammerTest.Logic
{
    public class Transaction
    {
        public enum State
        {
            Init,
            InProgress,
            FinishedSuccess,
            FinishedFailed
        }

        public enum Error
        {
            None,
            NotEnoughCurrency,
            StoreFailed,
            Unknown
        }

        public class TransactionFinishedEvent : UnityEvent<Transaction, bool> { }
        public TransactionFinishedEvent OnFinished = new TransactionFinishedEvent();

        public UserProfile.Currency TransactionCurrency { get; private set; } = UserProfile.Currency.Coins;
        public float Amount { get; private set; }
        public object Data { get; set; }
        public State TransactionState { get; private set; } = State.Init;
        public Error TransactionError { get; private set; } = Error.None;

        private const float _iapFailChancePercentage = 0.25f;
        private const float _iapMinDurationInSeconds = 0.5f;
        private const float _iapMaxDurationInSeconds = 5f;

        public void StartTransaction()
        {
            if (TransactionState != State.Init)
            {
                return;
            }

            TransactionState = State.InProgress;
            TransactionError = Error.None;

            switch (TransactionCurrency)
            {
                case UserProfile.Currency.Dollars:
                    {
                        System.Action finishTransaction = () =>
                        {
                            bool success = (Random.Range(0f, 1f) > _iapFailChancePercentage);
                            if (!success)
                            {
                                TransactionError = Error.StoreFailed;
                            }
                            FinishTransaction(success);
                        };
                        float randomDelay = Random.Range(_iapMinDurationInSeconds, _iapMaxDurationInSeconds);
                        CoroutineManager.DelayedCall(finishTransaction, randomDelay);
                    }
                    break;

                default:
                    {
                        float newBalance = UserProfile.Instance.GetCurrency(TransactionCurrency) + Amount;
                        bool enoughCurrency = (newBalance > 0);
                        if (enoughCurrency)
                        {
                            FinishTransaction(true);
                        }
                        else
                        {
                            TransactionError = Error.NotEnoughCurrency;
                            FinishTransaction(false);
                        }
                    }
                    break;
            }
        }

        public void ForceError(Error error)
        {
            bool hasTransactionFinished = (TransactionState == State.FinishedFailed || TransactionState == State.FinishedSuccess);
            if (hasTransactionFinished)
            {
                return;
            }

            TransactionError = error;
            FinishTransaction(error == Error.None);
        }

        private void FinishTransaction(bool success)
        {
            TransactionState = success ? State.FinishedSuccess : State.FinishedFailed;

            if (success)
            {
                UserProfile.Instance.ApplyTransaction(this);
            }

            OnFinished?.Invoke(this, success);
        }

        public static Transaction Create(UserProfile.Currency currency, float amount, object data = null)
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionCurrency = currency;
            newTransaction.Amount = amount;
            newTransaction.Data = data;
            return newTransaction;
        }

        private Transaction()
        {

        }
    }
}