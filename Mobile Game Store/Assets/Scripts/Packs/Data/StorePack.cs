using System;

namespace JGM.GameStore.Packs.Data
{
    public class StorePack
    {
        public enum State
        {
            PendingActivation,
            Active,
            Expired
        }

        public StorePackData PackData { get; private set; }
        public State PackState { get; private set; } = State.PendingActivation;
        public DateTime EndTimestamp { get; private set; } = DateTime.MaxValue;
        public TimeSpan RemainingTime => EndTimestamp - DateTime.UtcNow;

        public void Activate()
        {
            if (PackState != State.PendingActivation)
            {
                return;
            }

            EndTimestamp = DateTime.UtcNow + TimeSpan.FromMinutes(PackData.Duration);
            PackState = State.Active;
        }

        public void CheckExpiration()
        {
            if (PackState != State.Active)
            {
                return;
            }

            bool hasPackExpired = (PackData.IsTimed && RemainingTime.TotalSeconds < 0);
            if (hasPackExpired)
            {
                PackState = State.Expired;
            }
        }

        public void ApplyTransaction()
        {
            for (int i = 0; i < PackData.Items.Length; ++i)
            {
                PackData.Items[i].ApplyTransaction();
            }

            bool hasPackExpired = (PackData.PackType == StorePackData.Type.Offer);
            if (hasPackExpired)
            {
                PackState = State.Expired;
            }
        }

        public static StorePack CreateFromData(StorePackData data)
        {
            var newStorePack = new StorePack();
            newStorePack.PackData = data;
            return newStorePack;
        }

        public override string ToString()
        {
            string str = PackData.PackType + " " + PackData.TextId;
            str += " [" + PackData.Price + " " + PackData.PackCurrency + "]";

            if (PackData.IsTimed && PackState == State.Active)
            {
                str += "\n" + RemainingTime.ToString();
            }

            for (int i = 0; i < PackData.Items.Length; ++i)
            {
                str += "\n\t" + PackData.Items[i].ToString();
            }

            return str;
        }
    }
}