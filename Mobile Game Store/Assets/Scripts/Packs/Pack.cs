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

        public PackData Data { get; private set; }
        public State PackState { get; private set; } = State.PendingActivation;
        public DateTime EndTimestamp { get; private set; } = DateTime.MaxValue;
        public TimeSpan RemainingTime => EndTimestamp - DateTime.UtcNow;

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
            //for (int i = 0; i < Data.Items.Length; ++i)
            //{
            //    Data.Items[i].ApplyTransaction();
            //}

            bool hasPackExpired = (Data.PackType == PackData.Type.Offer);
            if (hasPackExpired)
            {
                PackState = State.Expired;
            }
        }

        public static Pack CreateFromData(PackData data)
        {
            var newStorePack = new Pack();
            newStorePack.Data = data;
            return newStorePack;
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
    }
}