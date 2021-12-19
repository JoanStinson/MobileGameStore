using JGM.GameStore.Packs.Data;
using static JGM.GameStore.Transaction.UserWallet;

namespace JGM.GameStore.Events
{
    public class PurchasePackEventData : IGameEventData
    {
        public StoreItemData[] Items;
        public Currency PackCurrency;
        public float Price;

        public PurchasePackEventData(in StoreItemData[] items, Currency currency, float price)
        {
            Items = items;
            PackCurrency = currency;
            Price = price;
        }
    }
}