// Transaction.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using UnityEngine;
using UnityEngine.Events;

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Class to control an async transaction operation.
	/// </summary>
	public class Transaction
	{
		#region CONSTANTS -----------------------------------------------------
		private const float IAPFailChance = 0.25f;    // Percentage
		private const float IAPMinDuration = 0.5f;    // Seconds
		private const float IAPMaxDuration = 5f;      // Seconds
		#endregion

		#region AUX CLASSES ---------------------------------------------------
		public enum State
		{
			Init,
			InProgress,
			FinishedSuccess,
			FinishedFailed
		}

		public enum Error
		{
			None,
			NotEnoughCurrency,
			StoreFailed,
			Unknown
		}

		public class TransactionFinishedEvent : UnityEvent<Transaction, bool> { }
		#endregion

		#region FIELDS AND PROPERTIES -----------------------------------------
		// Data
		private UserProfile.Currency m_currency = UserProfile.Currency.Coins;
		public UserProfile.Currency currency
		{
			get { return m_currency; }
		}

		private float m_amount = 0f;
		public float amount
		{
			get { return m_amount; }
		}

		private object m_data = null;
		public object data
		{
			get { return m_data; }
			set { m_data = value; }
		}

		// Logic
		private State m_state = State.Init;
		public State state
		{
			get { return m_state; }
		}

		private Error m_error = Error.None;
		public Error error
		{
			get { return m_error; }
		}

		// Events
		public TransactionFinishedEvent OnFinished = new TransactionFinishedEvent();
		#endregion

		#region PUBLIC METHODS ------------------------------------------------
		/// <summary>
		/// Start the transaction.
		/// </summary>
		public void Start()
		{
			// Only if not already started
			if (m_state != State.Init) return;

			// Change state
			m_state = State.InProgress;
			m_error = Error.None;

			// Perform different actions based on currency
			switch (m_currency)
			{
				// Real money: Simulate a store purchase with a delay
				case UserProfile.Currency.Dollars:
					{
						CoroutineManager.DelayedCall(
							() =>
							{
							// Success!
							bool success = Random.Range(0f, 1f) > IAPFailChance;    // X% chance of success
							if (!success) m_error = Error.StoreFailed;
								Finish(success);
							}, Random.Range(IAPMinDuration, IAPMaxDuration)
						);
					}
					break;

				// Rest of currencies: instant operation!
				default:
					{
						// Do we have enough currency?
						float newBalance = UserProfile.instance.GetCurrency(m_currency) + m_amount;
						if (newBalance < 0)
						{
							m_error = Error.NotEnoughCurrency;
							Finish(false);
						}
						else
						{
							// All good, perform the transaction!
							Finish(true);
						}
					}
					break;
			}
		}

		/// <summary>
		/// Force an error from outside. Will immediately finish the transaction.
		/// </summary>
		/// <param name="error">Error code.</param>
		public void ForceError(Error error)
		{
			// Ignore if transaction already finished
			if (m_state == State.FinishedFailed || m_state == State.FinishedSuccess)
			{
				return;
			}

			// Store new error and finish the transaction
			m_error = error;
			Finish(error == Error.None);
		}
		#endregion

		#region INTERNAL METHODS ----------------------------------------------
		/// <summary>
		/// Finish the transaction.
		/// </summary>
		/// <param name="success">Whether the transaction has been successful or not.</param>
		private void Finish(bool success)
		{
			// Change state
			m_state = success ? State.FinishedSuccess : State.FinishedFailed;

			// If successful, apply to user profile
			if (success)
			{
				UserProfile.instance.ApplyTransaction(this);
			}

			// Notify listeners
			OnFinished.Invoke(this, success);
		}
		#endregion

		#region FACTORY -------------------------------------------------------
		/// <summary>
		/// Private constructor, we want user to go through the factory method
		/// </summary>
		private Transaction()
		{

		}
		
		/// <summary>
		/// Create and initialize a new instance from a TransactionData object.
		/// </summary>
		/// <param name="currency">Currency for the new transaction.</param>
		/// <param name="amount">Amount of currency.</param>
		/// <param name="data">Optional extra data.</param>
		public static Transaction Create(UserProfile.Currency currency, float amount, object data = null)
		{
			// Create new transaction object
			Transaction newTransaction = new Transaction();

			// Initialize with data
			newTransaction.m_currency = currency;
			newTransaction.m_amount = amount;
			newTransaction.data = data;

			// Done!
			return newTransaction;
		}
		#endregion
	}
}
