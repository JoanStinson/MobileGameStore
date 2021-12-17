using System;
using Ubisoft.UIProgrammerTest.Utils;
using static Ubisoft.UIProgrammerTest.Singletons.UserProfile;

namespace Ubisoft.UIProgrammerTest.Data
{
    [Serializable]
    public class StorePackData
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
        public StoreItemData[] Items { get; protected set; } = null;

        private Type _packType = Type.Gems;
        private Currency _packCurrency = Currency.Gems;

        public static StorePackData CreateFromJson(JSONNode data)
        {
            var newStorePackData = new StorePackData();

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
                var itemsData = data["items"].AsArray;
                newStorePackData.Items = new StoreItemData[itemsData.Count];
                for (int i = 0; i < itemsData.Count; ++i)
                {
                    newStorePackData.Items[i] = StoreItemData.CreateFromJson(itemsData[i]);
                }
            }

            return newStorePackData;
        }
    }
}