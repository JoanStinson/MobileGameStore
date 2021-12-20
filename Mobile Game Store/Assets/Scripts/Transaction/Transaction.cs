using JGM.GameStore.Coroutines;
using JGM.GameStore.Transaction.User;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using static JGM.GameStore.Transaction.User.UserProfileService;

namespace JGM.GameStore.Transaction
{
    public partial class Transaction
    {
        public class Factory : PlaceholderFactory<Transaction> { }

        public class TransactionFinishedEvent : UnityEvent<Transaction, bool> { }
        public TransactionFinishedEvent OnFinished = new TransactionFinishedEvent();
        public State TransactionState { get; private set; } = State.Init;
        public Error TransactionError { get; private set; } = Error.None;

        public Currency TransactionCurrency { get; private set; }
        public float Amount { get; private set; }
        public object Data { get; private set; }

        private const float _iapFailChancePercentage = 0.25f;
        private const float _iapMinDurationInSeconds = 0.5f;
        private const float _iapMaxDurationInSeconds = 5f;

        [Inject] private ICoroutineService _coroutineService;
        [Inject] private IUserProfileService _userWallet;

        public void SetData(Currency currency, float amount, object data)
        {
            TransactionCurrency = currency;
            Amount = amount;
            Data = data;
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
                        _coroutineService.DelayedCall(finishTransaction, randomDelay);
                    }
                    break;

                case Currency.Gems:
                    {
                        System.Action finishTransaction = () =>
                        {
                            float newBalance = _userWallet.GetCurrency(TransactionCurrency) + Amount;
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
                        };
                        float randomDelay = Random.Range(_iapMinDurationInSeconds, _iapMaxDurationInSeconds);
                        _coroutineService.DelayedCall(finishTransaction, randomDelay);
                    }
                    break;

                case Currency.Coins:
                    {
                        float newBalance = _userWallet.GetCurrency(TransactionCurrency) + Amount;
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
                _userWallet.ApplyTransaction(this);
            }

            OnFinished?.Invoke(this, success);
        }
    }
}