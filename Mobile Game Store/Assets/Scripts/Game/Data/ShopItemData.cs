// ShopItemData.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Data container for a Shop Item.
	/// </summary>
	[System.Serializable]
	public class ShopItemData
	{
		#region AUX CLASSES ---------------------------------------------------
		public enum Type
		{
			Coins,
			Gems,
			Character
		}
		#endregion

		#region FIELDS AND PROPERTIES -----------------------------------------
		// Data
		private Type m_type = Type.Coins;
		public Type type
		{
			get { return m_type; }
		}

		public bool isCharacter
		{
			get { return type == Type.Character; }
		}

		private int m_amount = 0;   // Characters will have amount 1
		public int amount
		{
			get { return m_amount; }
		}

		private string m_itemId = "";   // Only for characters, ID of the character
		public string itemId
		{
			get { return m_itemId; }
		}

		// Visuals
		private string m_tidName = "";  // Text ID of the item
		public string tidName
		{
			get { return m_tidName; }
		}

		private string m_icon = "";     // Name of the icon sprite in the Resources folder
		public string icon
		{
			get { return m_icon; }
		}

		private string m_prefab = "";   // Name of the prefab of the item's 3D view in the Resources folder
		public string prefab
		{
			get { return m_prefab; }
		}
		#endregion

		#region PUBLIC METHODS ------------------------------------------------
		/// <summary>
		/// Apply this item to the user profile.
		/// Won't do any checks.
		/// </summary>
		public void Apply()
		{
			// Depending on type
			switch (type)
			{
				case Type.Coins:
				{
					Transaction trans = UserProfile.instance.CreateTransaction(UserProfile.Currency.Coins, amount);
					trans.Start();
				}
				break;

				case Type.Gems:
				{
					Transaction trans = UserProfile.instance.CreateTransaction(UserProfile.Currency.Gems, amount);
					trans.Start();
				}
				break;

				case Type.Character:
				{
					// Nothing to do actually
				}
				break;
			}
		}

		/// <summary>
		/// String formatting.
		/// </summary>
		public override string ToString()
		{
			return $"{{ {type} | {amount} | {itemId} }}";
		}
		#endregion

		#region FACTORY -------------------------------------------------------
		/// <summary>
		/// Private constructor.
		/// </summary>
		private ShopItemData()
		{

		}
		
		/// <summary>
		/// Create and initialize a new instance from Json data.
		/// </summary>
		/// <param name="data">Json data.</param>
		public static ShopItemData CreateFromJson(SimpleJSON.JSONNode data)
		{
			// Create the new object
			ShopItemData newObject = new ShopItemData();

			// Main data
			if (data.HasKey("type"))
			{
				Utils.EnumTryParse(data["type"], true, out newObject.m_type);
			}

			// Some other data depends on type
			if (newObject.isCharacter)
			{
				// Character: force amount to 1
				if (data.HasKey("itemId"))
				{
					newObject.m_itemId = data["itemId"];
				}

				newObject.m_amount = 1;
			}
			else
			{
				// Non-character: force id to ""
				if (data.HasKey("amount"))
				{
					newObject.m_amount = data["amount"].AsInt;
				}

				newObject.m_itemId = string.Empty;
			}

			// Visuals
			if (data.HasKey("tidName"))
			{
				newObject.m_tidName = data["tidName"];
			}

			if (data.HasKey("icon"))
			{
				newObject.m_icon = data["icon"];
			}

			if (data.HasKey("prefab"))
			{
				newObject.m_prefab = data["prefab"];
			}

			return newObject;
		}
		#endregion
	}
}
