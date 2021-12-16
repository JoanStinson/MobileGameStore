// SampleSceneController.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ubisoft.UIProgrammerTest.Sample
{
	/// <summary>
	/// Sample scene to demonstrat the basic functionality of the shop.
	/// </summary>
	public class SampleSceneController : MonoBehaviour
	{
		#region FIELDS AND PROPERTIES -----------------------------------------
		// Exposed references
		[SerializeField] private Text m_activePacksText = null;
		
		[Space]
		[SerializeField] private Text m_coinsText = null;
		[SerializeField] private Text m_gemsText = null;
		
		[Space]
		[SerializeField] private Text m_lastTransactionResult = null;
		[SerializeField] private Text m_busyText = null;
		[SerializeField] private GameObject m_busyPanel = null;

		// Internal logic
		private Transaction m_transaction = null;
		#endregion

		#region UNITY MESSAGES ------------------------------------------------
		/// <summary>
		/// First update call.
		/// </summary>
		private void Start()
		{
			// Initial refresh
			Refresh();
			m_lastTransactionResult.text = "";
		}

		/// <summary>
		/// Called every frame.
		/// </summary>
		private void Update()
		{
			// Not optimal, but enough for a test scene
			Refresh();
		}
		#endregion

		#region INTERNAL METHODS ----------------------------------------------
		/// <summary>
		/// Refresh visuals with latest data.
		/// </summary>
		private void Refresh()
		{
			// Busy screen: Do we have any active transaction?
			m_busyPanel.SetActive(m_transaction != null);
			if (m_transaction != null)
			{
				ShopPack pack = (ShopPack)m_transaction.data;
				m_busyText.text = "Purchasing pack " + pack.data.id;
			}

			// Active packs
			string str = "ACTIVE PACKS:\n\n";
			foreach (ShopPack pack in ShopManager.instance.activePacks)
			{
				str += pack.ToString() + "\n\n";
			}
			m_activePacksText.text = str;

			// Currency counters
			m_coinsText.text = LocalizationManager.instance.Localize("TID_COINS_NAME") + ": " + UserProfile.instance.GetCurrency(UserProfile.Currency.Coins).ToString();
			m_gemsText.text = LocalizationManager.instance.Localize("TID_GEMS_NAME") + ": " + UserProfile.instance.GetCurrency(UserProfile.Currency.Gems).ToString();
		}

		/// <summary>
		/// Buy a pack from the shop.
		/// </summary>
		/// <param name="pack">Pack to be boguht.</param>
		private void BuyPack(ShopPack pack)
		{
			// Ignore if a transaction is in progress
			if (m_transaction != null) return;

			// Launch a transaction
			m_transaction = UserProfile.instance.CreateTransaction(
				pack.data.currency,
				-pack.data.price       // Negative since we are subtracting!
			);

			// Add the pack as extra data
			m_transaction.data = pack;

			// Wait for transaction to be finished
			m_transaction.OnFinished.AddListener(OnTransactionFinished);

			// Launch transaction!
			m_transaction.Start();
		}
		#endregion

		#region CALLBACKS -----------------------------------------------------
		/// <summary>
		/// A transaction has finished.
		/// </summary>
		/// <param name="transaction">The transaction that triggered the event.</param>
		/// <param name="success">Whether the transaction was successful.</param>
		public void OnTransactionFinished(Transaction transaction, bool success)
		{
			// Is it our transaction?
			if (transaction == m_transaction)
			{
				// If successful, apply shop pack!
				ShopPack pack = (ShopPack)transaction.data;
				if (success)
				{
					pack.Apply();
				}

				// Show some feedback
				if (success)
				{
					m_lastTransactionResult.text = "<color=#00ff00>SUCCESS!</color>\n" + pack.data.id;
				}
				else
				{
					m_lastTransactionResult.text = "<color=#ff0000>FAILED! " + transaction.error.ToString() + "</color>\n" + pack.data.id;
				}

				// Clear transaction referenece
				m_transaction = null;
			}
		}

		/// <summary>
		/// Test button.
		/// </summary>
		public void OnBuyRandomPack()
		{
			// Select a random pack from all the active ones, regardless of the type
			List<ShopPack> pool = ShopManager.instance.activePacks;
			if (pool.Count > 0)
			{
				ShopPack targetPack = pool[Random.Range(0, pool.Count)];
				BuyPack(targetPack);
			}
		}

		/// <summary>
		/// Test button.
		/// </summary>
		public void OnBuyRandomOfferPack()
		{
			// Find all active offer packs
			List<ShopPack> allPacks = ShopManager.instance.activePacks;
			List<ShopPack> pool = new List<ShopPack>();
			for (int i = 0; i < allPacks.Count; ++i)
			{
				if (allPacks[i].data.type == ShopPackData.Type.Offer)
				{
					pool.Add(allPacks[i]);
				}
			}

			// Select a random one
			if (pool.Count > 0)
			{
				ShopPack targetPack = pool[Random.Range(0, pool.Count)];
				BuyPack(targetPack);
			}
		}

		/// <summary>
		/// Test button.
		/// </summary>
		public void OnBuyRandomCoinsPack()
		{
			// Find all active coin packs
			List<ShopPack> allPacks = ShopManager.instance.activePacks;
			List<ShopPack> pool = new List<ShopPack>();
			for (int i = 0; i < allPacks.Count; ++i)
			{
				if (allPacks[i].data.type == ShopPackData.Type.Coins)
				{
					pool.Add(allPacks[i]);
				}
			}

			// Select a random one
			if (pool.Count > 0)
			{
				ShopPack targetPack = pool[Random.Range(0, pool.Count)];
				BuyPack(targetPack);
			}
		}

		/// <summary>
		/// Test button.
		/// </summary>
		public void OnBuyRandomGemsPack()
		{
			// Find all active gem packs
			List<ShopPack> allPacks = ShopManager.instance.activePacks;
			List<ShopPack> pool = new List<ShopPack>();
			for (int i = 0; i < allPacks.Count; ++i)
			{
				if (allPacks[i].data.type == ShopPackData.Type.Gems)
				{
					pool.Add(allPacks[i]);
				}
			}

			// Select a random one
			if (pool.Count > 0)
			{
				ShopPack targetPack = pool[Random.Range(0, pool.Count)];
				BuyPack(targetPack);
			}
		}

		/// <summary>
		/// Test button.
		/// </summary>
		public void OnChangeLanguageRandom()
		{
			// Select a random language from the available ones in the localization manager
			LocalizationManager.Language newLang = (LocalizationManager.Language)Random.Range(0, (int)LocalizationManager.Language.Count);

			// Apply new language!
			LocalizationManager.instance.SetLanguage(newLang);

			// Force a refresh
			Refresh();
		}
		#endregion
	}
}
