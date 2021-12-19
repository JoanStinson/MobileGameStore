using static JGM.GameStore.Transaction.User.UserWallet;

namespace JGM.GameStore.Transaction.User
{
    public interface IUserWallet
    {
        float GetCurrency(Currency currency);
        Transaction CreateTransaction(Currency currency, float amount);
        void ApplyTransaction(Transaction transaction);
    }
}