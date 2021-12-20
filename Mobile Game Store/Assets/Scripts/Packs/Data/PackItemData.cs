using JGM.GameStore.Utils;
using System;

namespace JGM.GameStore.Packs.Data
{
    [Serializable]
    public class PackItemData
    {
        public enum Type
        {
            Coins,
            Gems,
            Character
        }

        public Type ItemType => _itemType;
        public bool IsCharacter => ItemType == Type.Character;
        public int Amount { get; private set; }
        public string ItemId { get; private set; }
        public string TextId { get; private set; }
        public string IconName { get; private set; }
        public string PrefabName { get; private set; }

        private Type _itemType = Type.Coins;

        public void PopulateDataFromJson(JSONNode json)
        {
            if (json.HasKey("type"))
            {
                DataParser.EnumTryParse(json["type"], true, out _itemType);
            }

            if (IsCharacter)
            {
                if (json.HasKey("itemId"))
                {
                    ItemId = json["itemId"];
                }

                Amount = 1;
            }
            else
            {
                if (json.HasKey("amount"))
                {
                    Amount = json["amount"].AsInt;
                }

                ItemId = string.Empty;
            }

            if (json.HasKey("tidName"))
            {
                TextId = json["tidName"];
            }

            if (json.HasKey("icon"))
            {
                IconName = json["icon"];
            }

            if (json.HasKey("prefab"))
            {
                PrefabName = json["prefab"];
            }
        }

        public override string ToString() => $"{{ {ItemType} | {Amount} | {ItemId} }}";
    }
}