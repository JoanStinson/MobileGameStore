// ShopPack.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using System;

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Logic controller for a single Shop Pack.
	/// </summary>
	public class ShopPack
	{
		#region AUX CLASSES ---------------------------------------------------
		public enum State
		{
			PendingActivation,
			Active,
			Expired
		}
		#endregion

		#region FIELDS AND PROPERTIES -----------------------------------------
		// Data
		private ShopPackData m_data = null;
		public ShopPackData data
		{
			get { return m_data; }
		}

		// Status
		private State m_state = State.PendingActivation;
		public State state
		{
			get { return m_state; }
		}

		// Timing
		private DateTime m_endTimestamp = DateTime.MaxValue;
		public DateTime endTimestamp
		{
			get { return m_endTimestamp; }
		}

		public TimeSpan remainingTime
		{
			get { return m_endTimestamp - DateTime.UtcNow; }
		}
		#endregion

		#region PUBLIC METHODS ------------------------------------------------
		/// <summary>
		/// Activate this shop pack.
		/// Will reset timestamps.
		/// Only valid for packs in PENDING_ACTIVATION state.
		/// </summary>
		public void Activate()
		{
			// Only for packs in the right state
			if (m_state != State.PendingActivation) return;

			// Calculate expiration timestiamp
			m_endTimestamp = DateTime.UtcNow + TimeSpan.FromMinutes(m_data.duration);

			// Change state
			m_state = State.Active;
		}

		/// <summary>
		/// Check whether this shop pack needs to be expired and mark it as expired if so.
		/// Only valid for packs in ACTIVE state.
		/// </summary>
		public void CheckExpiration()
		{
			// Only active packs
			if (m_state != State.Active) return;

			// If pack is timed and expiration date has passed, mark it as expired
			if (m_data.isTimed && remainingTime.TotalSeconds < 0)
			{
				m_state = State.Expired;
			}
		}

		/// <summary>
		/// Apply this pack's rewards to the current User Profile.
		/// Won't do any checks!
		/// </summary>
		public void Apply()
		{
			// Apply rewards!
			for (int i = 0; i < m_data.items.Length; ++i)
			{
				m_data.items[i].Apply();
			}

			// If offer pack, mark it as expired so the pack is removed from the manager
			if (m_data.type == ShopPackData.Type.Offer)
			{
				m_state = State.Expired;
			}
		}

		/// <summary>
		/// String representation of this pack.
		/// </summary>
		public override string ToString()
		{
			// Pack Type + ID
			string str = data.type + " " + data.id;

			// Price
			str += " [" + data.price + " " + data.currency + "]";

			// Remaining time
			if (data.isTimed && state == State.Active)
			{
				str += "\n" + remainingTime.ToString();
			}

			// Items
			for (int i = 0; i < data.items.Length; ++i)
			{
				str += "\n\t" + data.items[i].ToString();
			}

			return str;
		}
		#endregion

		#region FACTORY -------------------------------------------------------
		/// <summary>
		/// Create and initialize a new instance from a ShopPackData object.
		/// </summary>
		/// <param name="data">The data object.</param>
		public static ShopPack CreateFromData(ShopPackData data)
		{
			// Create new object
			ShopPack newObject = new ShopPack();

			// Load data
			newObject.m_data = data;

			// Done!
			return newObject;
		}
		#endregion
	}
}
