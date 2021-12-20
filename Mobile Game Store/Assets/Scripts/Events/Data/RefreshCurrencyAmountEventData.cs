namespace JGM.GameStore.Events.Data
{
    public class RefreshCurrencyAmountEventData : IGameEventData
    {
        public readonly float Amount;

        public RefreshCurrencyAmountEventData(float amount)
        {
            Amount = amount;
        }
    }
}