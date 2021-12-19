using JGM.GameStore.Packs.Data;
using System;

namespace JGM.GameStore.Packs
{
    public class Pack
    {
        public enum State
        {
            PendingActivation,
            Active,
            Expired
        }

        public PackData PackData { get; private set; }
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

            bool hasPackExpired = (PackData.PackType == PackData.Type.Offer);
            if (hasPackExpired)
            {
                PackState = State.Expired;
            }
        }

        public static Pack CreateFromData(PackData data)
        {
            var newStorePack = new Pack();
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