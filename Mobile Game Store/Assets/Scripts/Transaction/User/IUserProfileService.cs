using JGM.GameStore.Coroutines;
using static JGM.GameStore.Transaction.User.UserProfileService;

namespace JGM.GameStore.Transaction.User
{
    public interface IUserProfileService
    {
        float GetCurrency(Currency currency);
        Transaction CreateTransaction(Currency currency, float amount, object data = null);
        void ApplyTransaction(in Transaction transaction);
    }
}