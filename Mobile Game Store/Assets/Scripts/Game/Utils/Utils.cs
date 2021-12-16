// Utils.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using System;

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Collection of static generic utils.
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Converts the string representation of the name or numeric value of one or 
		/// more enumerated constants to an equivalent enumerated object. A parameter 
		/// specifies whether the operation is case-sensitive. The return value indicates 
		/// whether the conversion succeeded.
		/// </summary>
		/// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully, <c>false</c> otherwise.</returns>
		/// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
		/// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to consider case.</param>
		/// <param name="result">When this method returns, <paramref name="result"/> contains an object of type <typeparamref name="T"/> whose value is represented by <paramref name="value"/> if the parse operation succeeds. If the parse operation fails, <paramref name="result"/> contains the default value of the underlying type of <typeparamref name="T"/>. Note that this value need not be a member of the <typeparamref name="T"/> enumeration. This parameter is passed uninitialized.</param>
		/// <typeparam name="T">The enumeration type to which to convert <paramref name="value"/>.</typeparam>
		public static bool EnumTryParse<T>(string value, bool ignoreCase, out T result) where T : struct
		{
			try
			{
				result = (T)Enum.Parse(typeof(T), value, ignoreCase);
				return true;
			}
			catch
			{
				result = default(T);
				return false;
			}
		}
	}
}
