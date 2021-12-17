namespace JGM.GameStore.Transaction
{
    public partial class Transaction
    {
        public enum Error
        {
            None,
            NotEnoughCurrency,
            StoreFailed,
            Unknown
        }
    }
}