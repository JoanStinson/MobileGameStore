using JGM.GameStore.Coroutines;
using static JGM.GameStore.Transaction.UserProfileService;

namespace JGM.GameStore.Transaction
{
    public interface IUserProfileService
    {
        float GetCurrency(Currency currency);
        Transaction CreateTransaction(Currency currency, float amount, object data = null);
        void ApplyTransaction(in Transaction transaction);
    }
}