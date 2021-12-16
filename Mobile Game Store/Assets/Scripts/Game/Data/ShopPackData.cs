// ShopPackData.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Data container for a Shop Pack
	/// </summary>
	[System.Serializable]
	public class ShopPackData
	{
		#region AUX CLASSES ---------------------------------------------------
		public enum Type
		{
			Coins,
			Gems,
			Offer
		}
		#endregion

		#region FIELDS AND PROPERTIES -----------------------------------------
		// Data
		protected string m_id = "";
		public string id
		{
			get { return m_id; }
		}

		protected Type m_type = Type.Coins;
		public Type type
		{
			get { return m_type; }
		}

		// Secondary data
		protected int m_order = 0;  // Order in the shop
		public int order
		{
			get { return m_order; }
		}

		protected float m_duration = -1f; // Minutes, negative if it doesn't expire
		public float duration
		{
			get { return m_duration; }
		}

		public bool isTimed
		{
			get { return m_duration > 0f; }
		}

		// Visuals
		protected string m_tidName = "";    // Text ID of the pack name
		public string tidName
		{
			get { return m_tidName; }
		}

		protected bool m_featured = false;  // Put in the featured spot?
		public bool featured
		{
			get { return m_featured; }
		}

		// Pricing
		protected float m_price = 0;
		public float price
		{
			get { return m_price; }
		}

		public float priceBeforeDiscount
		{
			get { return m_price / (1f - m_discount); }
		}

		protected UserProfile.Currency m_currency = UserProfile.Currency.Gems;
		public UserProfile.Currency currency
		{
			get { return m_currency; }
		}

		protected float m_discount = 0f;    // [0..1] Percentage of discount
		public float discount
		{
			get { return m_discount; }
		}

		// Items
		protected ShopItemData[] m_items = null;
		public ShopItemData[] items
		{
			get { return m_items; }
		}
		#endregion

		#region FACTORY ----------------------------------------------------------
		/// <summary>
		/// Create and initialize a new instance from Json data.
		/// </summary>
		/// <param name="data">Json data.</param>
		public static ShopPackData CreateFromJson(SimpleJSON.JSONNode data)
		{
			// Create the new object
			ShopPackData newObject = new ShopPackData();

			// Main data
			if (data.HasKey("id"))
			{
				newObject.m_id = data["id"];
			}

			if (data.HasKey("type"))
			{
				Utils.EnumTryParse(data["type"], true, out newObject.m_type);
			}

			// Secondary data
			if (data.HasKey("order"))
			{
				newObject.m_order = data["order"].AsInt;
			}

			if (data.HasKey("duration"))
			{
				newObject.m_duration = data["duration"].AsFloat;
			}

			// Visuals
			if (data.HasKey("tidName"))
			{
				newObject.m_tidName = data["tidName"];
			}

			if (data.HasKey("featured"))
			{
				newObject.m_featured = data["featured"].AsBool;
			}

			// Pricing
			if (data.HasKey("price"))
			{
				newObject.m_price = data["price"].AsFloat;
			}

			if (data.HasKey("currency"))
			{
				Utils.EnumTryParse(data["currency"], true, out newObject.m_currency);
			}

			if (data.HasKey("discount"))
			{
				newObject.m_discount = data["discount"].AsFloat;
			}

			// Items
			if (data.HasKey("items"))
			{
				SimpleJSON.JSONArray itemsData = data["items"].AsArray;
				newObject.m_items = new ShopItemData[itemsData.Count];
				for (int i = 0; i < itemsData.Count; ++i)
				{
					newObject.m_items[i] = ShopItemData.CreateFromJson(itemsData[i]);
				}
			}

			return newObject;
		}
		#endregion
	}
}
