// UserProfile.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using UnityEngine;
using UnityEngine.Events;

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Class storing data from the player.
	/// </summary>
	public class UserProfile
	{
		#region AUX CLASSES ---------------------------------------------------
		public enum Currency
		{
			Coins = 0,
			Gems,
			Dollars,

			Count
		}

		public class CurrencyEvent : UnityEvent<Currency, float> { }
		#endregion

		#region SINGLETON -----------------------------------------------------
		// Singleton instance
		private static UserProfile s_instance = null;
		public static UserProfile instance
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
				s_instance = new UserProfile();
			}
		}
		#endregion

		#region FIELDS AND PROPERTIES -----------------------------------------
		// Currencies
		private float[] m_currencies = new float[(int)Currency.Count];

		// Events
		public CurrencyEvent OnCurrencyChanged = new CurrencyEvent();
		#endregion

		#region PUBLIC METHODS ------------------------------------------------
		/// <summary>
		/// Get the current amount of a currency.
		/// </summary>
		/// <returns>The current amount of the requested currency.</returns>
		/// <param name="currency">Currency.</param>
		public float GetCurrency(Currency currency)
		{
			return m_currencies[(int)currency];
		}

		/// <summary>
		/// Create a transaction to add/Subtract currency to the profile.
		/// The new transaction needs to be started manually. Once successful, the target currency will be updated.
		/// </summary>
		/// <returns>A Transaction object to control the state of the transaction.</returns>
		/// <param name="currency">Which currency?</param>
		/// <param name="amount">Amount. Use negative value to subtract.</param>
		public Transaction CreateTransaction(Currency currency, float amount)
		{
			// Create the transaction object
			Transaction t = Transaction.Create(currency, amount);

			// Return the new transaction
			return t;
		}

		/// <summary>
		/// Apply a transaction. Only if transaction is successful.
		/// </summary>
		/// <param name="trans">The transaction to be applied.</param>
		public void ApplyTransaction(Transaction trans)
		{
			// Only if transaction is successful
			if (trans.state != Transaction.State.FinishedSuccess) return;

			// Apply the currency change
			m_currencies[(int)trans.currency] = GetCurrency(trans.currency) + trans.amount;

			// Notify listeners
			OnCurrencyChanged.Invoke(trans.currency, trans.amount);
		}
		#endregion
	}
}
