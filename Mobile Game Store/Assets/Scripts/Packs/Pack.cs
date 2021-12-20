using JGM.GameStore.Coroutines;
using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Transaction.User;
using System;
using Zenject;
using static JGM.GameStore.Transaction.User.UserProfileService;

namespace JGM.GameStore.Packs
{
    public class Pack
    {
        public class Factory : PlaceholderFactory<Pack> { }

        public enum State
        {
            PendingActivation,
            Active,
            Expired
        }

        public PackData Data { get; private set; }
        public State PackState { get; private set; } = State.PendingActivation;
        public DateTime EndTimestamp { get; private set; } = DateTime.MaxValue;
        public TimeSpan RemainingTime => EndTimestamp - DateTime.UtcNow;

        [Inject] private IUserProfileService _userWallet;
        [Inject] private IEventTriggerService _eventTriggerService;

        public void SetData(PackData data)
        {
            Data = data;
        }

        public void Activate()
        {
            if (PackState != State.PendingActivation)
            {
                return;
            }

            EndTimestamp = DateTime.UtcNow + TimeSpan.FromMinutes(Data.Duration);
            PackState = State.Active;
        }

        public void CheckExpiration()
        {
            if (PackState != State.Active)
            {
                return;
            }

            bool hasPackExpired = (Data.IsTimed && RemainingTime.TotalSeconds < 0);
            if (hasPackExpired)
            {
                PackState = State.Expired;
            }
        }

        public void ApplyTransaction()
        {
            for (int i = 0; i < Data.Items.Length; ++i)
            {
                switch (Data.Items[i].ItemType)
                {
                    case PackItemData.Type.Coins:
                        {
                            var coinsTransaction = _userWallet.CreateTransaction(Currency.Coins, Data.Items[i].Amount);
                            coinsTransaction.OnFinished.AddListener((transaction, success) => SendRefreshCurrencyEvent(transaction, success, "Refresh Coins Amount"));
                            coinsTransaction.StartTransaction();
                        }
                        break;

                    case PackItemData.Type.Gems:
                        {
                            var gemsTransaction = _userWallet.CreateTransaction(Currency.Gems, Data.Items[i].Amount);
                            gemsTransaction.OnFinished.AddListener((transaction, success) => SendRefreshCurrencyEvent(transaction, success, "Refresh Gems Amount"));
                            gemsTransaction.StartTransaction();
                        }
                        break;

                    case PackItemData.Type.Character:
                        {
                            // Nothing to do actually
                        }
                        break;
                }
            }

            bool hasPackExpired = (Data.PackType == PackData.Type.Offer);
            if (hasPackExpired)
            {
                PackState = State.Expired;
            }
        }

        public override string ToString()
        {
            string str = Data.PackType + " " + Data.TextId;
            str += " [" + Data.Price + " " + Data.PackCurrency + "]";

            if (Data.IsTimed && PackState == State.Active)
            {
                str += "\n" + RemainingTime.ToString();
            }

            for (int i = 0; i < Data.Items.Length; ++i)
            {
                str += "\n\t" + Data.Items[i].ToString();
            }

            return str;
        }

        private void SendRefreshCurrencyEvent(in Transaction.Transaction transaction, bool success, in string eventName)
        {
            if (success)
            {
                var eventData = new RefreshCurrencyAmountEventData(transaction.Amount);
                _eventTriggerService.Trigger(eventName, eventData);
            }
        }
    }
}