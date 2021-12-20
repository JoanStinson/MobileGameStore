using JGM.GameStore.Coroutines;
using static JGM.GameStore.Transaction.User.UserWallet;

namespace JGM.GameStore.Transaction.User
{
    public interface IUserWallet
    {
        float GetCurrency(Currency currency);
        Transaction CreateTransaction(Currency currency, float amount, object data, ICoroutineService coroutineService, IUserWallet userWallet);
        void ApplyTransaction(Transaction transaction);
    }
}