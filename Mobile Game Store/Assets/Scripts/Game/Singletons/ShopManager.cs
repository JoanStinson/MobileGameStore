// ShopPacksManager.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Main manager for shop packs.
	/// Monobehaviour since we need the Update() call.
	/// </summary>
	public class ShopManager : MonoBehaviour
	{
		#region CONSTANTS -----------------------------------------------------
		// Settings
		private const float RefreshFrequency = 1f;
		private const int ActiveOfferPacks = 3;
		private const int OffersHistoryMaxSize = ActiveOfferPacks + 1; // Prevent repeating the last used offer pack
		#endregion

		#region AUX CLASSES ---------------------------------------------------
		// Auxiliar classes
		public class ShopPackEvent : UnityEvent<ShopPack> { }
		#endregion

		#region SINGLETON -----------------------------------------------------
		// Singleton instance
		private static ShopManager s_instance = null;
		public static ShopManager instance
		{
			get
			{
				ValidateSingletonInstance();
				return s_instance;
			}
		}

		/// <summary>
		/// Static method to automatically create the singleton instance if needed.
		/// Will be automatically invoked upon app initialization.
		/// </summary>
		[RuntimeInitializeOnLoadMethod]
		private static void ValidateSingletonInstance()
		{
			if (s_instance == null)
			{
				// Create the object and give it the name of the class
				GameObject singletonObj = new GameObject(typeof(ShopManager).Name);

				// Create the instance by adding it as a component of the game object we just created
				// Store its reference so this is only done once
				s_instance = singletonObj.AddComponent<ShopManager>();

				// Prevents this game object which has been created by scripts to be saved in the scene if a instance stayed in the scene after playing by mistake
				s_instance.hideFlags = HideFlags.DontSave;
			}
		}
		#endregion

		#region FIELDS AND PROPERTIES -----------------------------------------
		// Collections
		private List<ShopPack> m_activePacks = new List<ShopPack>();    // Not sorted
		public List<ShopPack> activePacks
		{
			get { return m_activePacks; }
		}
		private List<ShopPack> m_activeOfferPacks = new List<ShopPack>();
		private List<ShopPackData> m_offerPacksDatabase = new List<ShopPackData>();
		private Queue<string> m_offerPacksHistory = new Queue<string>();

		// Events
		public ShopPackEvent OnPackActivated = new ShopPackEvent();
		public ShopPackEvent OnPackRemoved = new ShopPackEvent();
		#endregion

		#region UNITY MESSAGES ------------------------------------------------
		/// <summary>
		/// Initialization.
		/// </summary>
		private void Awake()
		{
			Load();
		}

		/// <summary>
		/// First update.
		/// </summary>
		private void Start()
		{
			// Program periodic update
			InvokeRepeating("UpdatePeriodic", 0f, RefreshFrequency);
		}

		/// <summary>
		/// Update loop.
		/// </summary>
		private void UpdatePeriodic()
		{
			// Refresh packs!
			Refresh();
		}
		#endregion

		#region INTERNAL METHODS ----------------------------------------------
		/// <summary>
		/// Will refresh the list of offers to be displayed.
		/// </summary>
		private void Refresh()
		{
			// Aux vars
			List<ShopPack> toRemove = new List<ShopPack>();

			// Loop through all active packs and check those that need to be expired
			for (int i = 0; i < m_activePacks.Count; ++i)
			{
				// Needs to be expired?
				m_activePacks[i].CheckExpiration();
				if (m_activePacks[i].state == ShopPack.State.Expired)
				{
					toRemove.Add(m_activePacks[i]);
				}
			}

			// Remove all the packs requiring so
			for (int i = 0; i < toRemove.Count; ++i)
			{
				RemovePack(toRemove[i]);
			}

			// Do we need to activate a new offer pack?
			int loopCount = 50; // Just in case, prevent infinite loop
			while (m_activeOfferPacks.Count < ActiveOfferPacks && loopCount > 0)
			{
				// Decrease loop counter
				loopCount--;

				// Create a pool of selectable packs
				List<ShopPackData> pool = new List<ShopPackData>();
				for (int i = 0; i < m_offerPacksDatabase.Count; ++i)
				{
					// Don't use this pack if it has been used recently
					if (m_offerPacksHistory.Contains(m_offerPacksDatabase[i].id)) continue;

					// All checks passed! Add pack to the pool
					pool.Add(m_offerPacksDatabase[i]);
				}

				// Do we have any valid candidates?
				if (pool.Count > 0)
				{
					// Yes!! Pick a random pack from the pool and activate it!
					ShopPackData newPackData = pool[Random.Range(0, pool.Count)];
					CreateAndActivatePack(newPackData);
				}
				else
				{
					// No!! (shouldn't happen) Remove last entry from the history and skip to next loop
					m_offerPacksHistory.Dequeue();
					continue;
				}
			}
		}

		/// <summary>
		/// Create and activate a pack with the given data.
		/// </summary>
		/// <param name="_packData">Pack data.</param>
		private void CreateAndActivatePack(ShopPackData _packData)
		{
			// Create a new pack with given data
			ShopPack newPack = ShopPack.CreateFromData(_packData);

			// Put it in the target collections
			m_activePacks.Add(newPack);

			// Some extra processing required for offer packs
			if (newPack.data.type == ShopPackData.Type.Offer)
			{
				// Add it to the active offer packs
				m_activeOfferPacks.Add(newPack);

				// Put it to the offers history
				m_offerPacksHistory.Enqueue(_packData.id);

				// Purge history until we have the right size
				while (m_offerPacksHistory.Count > OffersHistoryMaxSize)
				{
					m_offerPacksHistory.Dequeue();
				}
			}

			// Activate pack!
			newPack.Activate();

			// Notify listeners
			OnPackActivated.Invoke(newPack);
		}

		/// <summary>
		/// Remove the target pack from the manager.
		/// </summary>
		/// <param name="pack">Pack to be removed.</param>
		private void RemovePack(ShopPack pack)
		{
			// Remove it for collections
			m_activePacks.Remove(pack);
			m_activeOfferPacks.Remove(pack);

			// Notify listeners
			OnPackRemoved.Invoke(pack);
		}

		/// <summary>
		/// Initialize with data.
		/// </summary>
		private void Load()
		{
			// Read data file from disk
			TextAsset txt = Resources.Load<TextAsset>("Data/shop_manager");

			// Parse json data
			SimpleJSON.JSONNode data = SimpleJSON.JSONNode.Parse(txt.text);

			// Load all packs data
			m_offerPacksDatabase.Clear();
			m_activeOfferPacks.Clear();
			m_activePacks.Clear();
			if (data.HasKey("packs"))
			{
				SimpleJSON.JSONArray packsData = data["packs"].AsArray;
				for (int i = 0; i < packsData.Count; ++i)
				{
					// Create pack data
					ShopPackData packData = ShopPackData.CreateFromJson(packsData[i]);

					// Is it an offer pack?
					if (packData.type != ShopPackData.Type.Offer)
					{
						// No! Activate it directly
						CreateAndActivatePack(packData);
					}
					else
					{
						// Yes! Put it in the offers pool and it will be automatically activated when needed
						m_offerPacksDatabase.Add(packData);
					}
				}
			}
		}
		#endregion
	}
}
