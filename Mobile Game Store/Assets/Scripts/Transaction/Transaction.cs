using JGM.GameStore.Coroutines;
using JGM.GameStore.Transaction.User;
using UnityEngine;
using UnityEngine.Events;
using static JGM.GameStore.Transaction.User.UserWallet;

namespace JGM.GameStore.Transaction
{
    public partial class Transaction
    {
        public class TransactionFinishedEvent : UnityEvent<Transaction, bool> { }
        public TransactionFinishedEvent OnFinished = new TransactionFinishedEvent();
        public Currency TransactionCurrency { get; private set; } = Currency.Coins;
        public float Amount { get; private set; }
        public object Data { get; private set; }
        public ICoroutineService CoroutineService { get; private set; }
        public IUserWallet UserWallet { get; private set; }
        public State TransactionState { get; private set; } = State.Init;
        public Error TransactionError { get; private set; } = Error.None;

        private const float _iapFailChancePercentage = 0.25f;
        private const float _iapMinDurationInSeconds = 0.5f;
        private const float _iapMaxDurationInSeconds = 5f;

        public static Transaction Create(Currency currency, float amount, object data, ICoroutineService coroutineService, IUserWallet userWallet)
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionCurrency = currency;
            newTransaction.Amount = amount;
            newTransaction.Data = data;
            newTransaction.CoroutineService = coroutineService;
            newTransaction.UserWallet = userWallet;
            return newTransaction;
        }

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
                case Currency.Dollars:
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
                        CoroutineService.DelayedCall(finishTransaction, randomDelay);
                    }
                    break;

                default:
                    {
                        float newBalance = UserWallet.GetCurrency(TransactionCurrency) + Amount;
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
                UserWallet.ApplyTransaction(this);
            }

            OnFinished?.Invoke(this, success);
        }

        private Transaction()
        {

        }
    }
}