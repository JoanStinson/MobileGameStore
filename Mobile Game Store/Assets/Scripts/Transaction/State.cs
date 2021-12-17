namespace JGM.GameStore.Transaction
{
    public partial class Transaction
    {
        public enum State
        {
            Init,
            InProgress,
            FinishedSuccess,
            FinishedFailed
        }
    }
}