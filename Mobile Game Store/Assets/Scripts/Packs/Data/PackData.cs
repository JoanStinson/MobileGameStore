using JGM.GameStore.Utils;
using System;
using static JGM.GameStore.Transaction.User.UserProfileService;

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

        public void PopulateDataFromJson(JSONNode json)
        {
            if (json.HasKey("id"))
            {
                Id = json["id"];
            }

            if (json.HasKey("type"))
            {
                DataParser.EnumTryParse(json["type"], true, out _packType);
            }

            if (json.HasKey("order"))
            {
                Order = json["order"].AsInt;
            }

            if (json.HasKey("duration"))
            {
                Duration = json["duration"].AsFloat;
            }

            if (json.HasKey("tidName"))
            {
                TextId = json["tidName"];
            }

            if (json.HasKey("featured"))
            {
                Featured = json["featured"].AsBool;
            }

            if (json.HasKey("price"))
            {
                Price = json["price"].AsFloat;
            }

            if (json.HasKey("currency"))
            {
                DataParser.EnumTryParse(json["currency"], true, out _packCurrency);
            }

            if (json.HasKey("discount"))
            {
                Discount = json["discount"].AsFloat;
            }

            if (json.HasKey("items"))
            {
                JSONNode itemsData = json["items"].AsArray;
                Items = new PackItemData[itemsData.Count];
                for (int i = 0; i < itemsData.Count; ++i)
                {
                    Items[i] = new PackItemData();
                    Items[i].PopulateDataFromJson(itemsData[i]);
                }
            }
        }
    }
}