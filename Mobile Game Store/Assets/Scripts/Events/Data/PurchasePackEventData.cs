using JGM.GameStore.Packs.Data;
using static JGM.GameStore.Transaction.User.UserWallet;

namespace JGM.GameStore.Events.Data
{
    public class PurchasePackEventData : IGameEventData
    {
        public PackItemData[] Items;
        public Currency PackCurrency;
        public float Price;

        public PurchasePackEventData(in PackItemData[] items, Currency currency, float price)
        {
            Items = items;
            PackCurrency = currency;
            Price = price;
        }
    }
}