using JGM.GameStore.Packs.Data;
using static JGM.GameStore.Packs.Data.PackItemData;
using static JGM.GameStore.Transaction.User.UserWallet;

namespace JGM.GameStore.Events.Data
{
    public class PurchasePackEventData : IGameEventData
    {
        public readonly PackItemData[] Items;
        public readonly Currency PackCurrency;
        public readonly float Price;
        //public readonly Type ItemType;

        public PurchasePackEventData(in PackItemData[] items, Currency currency, float price)
        {
            Items = items;
            PackCurrency = currency;
            Price = price;
            //ItemType = type;
        }
    }
}