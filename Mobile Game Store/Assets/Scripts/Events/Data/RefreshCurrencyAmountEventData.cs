namespace JGM.GameStore.Events.Data
{
    public interface ICurrencyEventData : IGameEventData
    {
        float Amount { get; }
    }

    public class RefreshCurrencyAmountEventData : ICurrencyEventData
    {
        public float Amount { get; private set; }

        public RefreshCurrencyAmountEventData(float amount)
        {
            Amount = amount;
        }
    }
}