using JGM.GameStore.Utils;
using System;
using static JGM.GameStore.Transaction.User.UserWallet;

namespace JGM.GameStore.Packs.Data
{
    [Serializable]
    public class PackData
    {
        public enum Type
        {
            Coins,
            Gems,
            Offer
        }

        public string Id { get; protected set; }
        public Type PackType => _packType;
        public int Order { get; protected set; }
        public float Duration { get; protected set; } = -1f;
        public bool IsTimed => Duration > 0f;
        public string TextId { get; protected set; }
        public bool Featured { get; protected set; }
        public float Price { get; protected set; }
        public float PriceBeforeDiscount => Price / (1f - Discount);
        public Currency PackCurrency => _packCurrency;
        public float Discount { get; protected set; }
        public PackItemData[] Items { get; protected set; } = null;

        private Type _packType = Type.Gems;
        private Currency _packCurrency = Currency.Gems;

        public static PackData CreateFromJson(JSONNode data)
        {
            var newStorePackData = new PackData();

            if (data.HasKey("id"))
            {
                newStorePackData.Id = data["id"];
            }

            if (data.HasKey("type"))
            {
                DataParser.EnumTryParse(data["type"], true, out newStorePackData._packType);
            }

            if (data.HasKey("order"))
            {
                newStorePackData.Order = data["order"].AsInt;
            }

            if (data.HasKey("duration"))
            {
                newStorePackData.Duration = data["duration"].AsFloat;
            }

            if (data.HasKey("tidName"))
            {
                newStorePackData.TextId = data["tidName"];
            }

            if (data.HasKey("featured"))
            {
                newStorePackData.Featured = data["featured"].AsBool;
            }

            if (data.HasKey("price"))
            {
                newStorePackData.Price = data["price"].AsFloat;
            }

            if (data.HasKey("currency"))
            {
                DataParser.EnumTryParse(data["currency"], true, out newStorePackData._packCurrency);
            }

            if (data.HasKey("discount"))
            {
                newStorePackData.Discount = data["discount"].AsFloat;
            }

            if (data.HasKey("items"))
            {
                JSONNode itemsData = data["items"].AsArray;
                newStorePackData.Items = new PackItemData[itemsData.Count];
                for (int i = 0; i < itemsData.Count; ++i)
                {
                    newStorePackData.Items[i] = PackItemData.CreateFromJson(itemsData[i]);
                }
            }

            return newStorePackData;
        }
    }
}