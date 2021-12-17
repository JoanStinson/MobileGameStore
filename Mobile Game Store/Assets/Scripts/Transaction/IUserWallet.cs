using static JGM.GameStore.Transaction.UserWallet;

namespace JGM.GameStore.Transaction
{
    public interface IUserWallet
    {
        float GetCurrency(Currency currency);
        Transaction CreateTransaction(Currency currency, float amount);
        void ApplyTransaction(Transaction transaction);
    }
}