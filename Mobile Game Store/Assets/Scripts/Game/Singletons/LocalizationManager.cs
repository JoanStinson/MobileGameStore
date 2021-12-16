// LocalizationManager.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Centralized class to handle all localization management.
	/// </summary>
	public class LocalizationManager
	{
		#region CONSTANTS -----------------------------------------------------
		private const Language DefaultLanguage = Language.English;
		private const string DataFolder = "Localization/";
		#endregion

		#region AUX CLASSES ---------------------------------------------------
		public enum Language
		{
			English,
			French,
			Italian,
			German,
			Spanish,
			Portuguese,
			Russian,
			Chinese,
			Japanese,
			Korean,

			Count
		}

		/// <summary>
		/// Data for a single language.
		/// </summary>
		public class LangData
		{
			public Language language = Language.Count;
			public string isoCode = "";
			public string fontName = "";
			public Dictionary<string, string> dict = new Dictionary<string, string>();
		}

		/// <summary>
		/// Event for when the active language has changed.
		/// </summary>
		public class LanguageChangedEvent : UnityEvent<Language, Language> { }
		#endregion

		#region SINGLETON -----------------------------------------------------
		// Singleton instance
		private static LocalizationManager s_instance = null;
		public static LocalizationManager instance
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
				s_instance = new LocalizationManager();
			}
		}
		#endregion

		#region FIELDS AND PROPERTIES -----------------------------------------
		// Languages
		private Language m_currentLanguage = Language.Count;
		public Language currentLanguage
		{
			get { return m_currentLanguage; }
		}

		// Dictionary
		private Dictionary<Language, LangData> m_languages = null;
		private LangData currentLanguageData
		{
			get { return m_languages[m_currentLanguage]; }
		}

		// Events
		public LanguageChangedEvent OnLanguageChanged = new LanguageChangedEvent();
		#endregion

		#region METHODS -------------------------------------------------------
		/// <summary>
		/// Private constructor. Accessible by the singleton instance.
		/// </summary>
		private LocalizationManager()
		{
			// Read data file from disk
			TextAsset txt = Resources.Load<TextAsset>("Data/localization_manager");

			// Parse json data
			SimpleJSON.JSONNode jsonData = SimpleJSON.JSONNode.Parse(txt.text);

			// Load all languages
			m_languages = new Dictionary<Language, LangData>();
			for (Language lang = Language.English; lang < Language.Count; ++lang)
			{
				// Create a new data object for this language and store it in the dictionary
				LangData newLangData = new LangData();
				newLangData.language = lang;
				m_languages[lang] = newLangData;

				// Do we have data for this language?
				string langKey = lang.ToString().ToLowerInvariant();
				if (jsonData.HasKey(langKey))
				{
					// Yes!! Parse it
					ParseLangData(ref newLangData, jsonData[langKey]);
				}
			}

			// Set default language as the current one
			SetLanguage(DefaultLanguage);
		}

		/// <summary>
		/// Change the current language.
		/// </summary>
		/// <param name="lang">The new language.</param>
		public void SetLanguage(Language lang)
		{
			// Ignore if not valid
			if (lang == Language.Count) return;

			// Store new language
			Language oldLang = m_currentLanguage;
			m_currentLanguage = lang;

			// Broadcast
			OnLanguageChanged.Invoke(oldLang, m_currentLanguage);
		}

		/// <summary>
		/// Given a text ID, return it localized in the current language.
		/// </summary>
		/// <returns>The localized test.</returns>
		/// <param name="tid">ID of the text to be localized.</param>
		public string Localize(string tid)
		{
			if (currentLanguageData.dict.ContainsKey(tid))
			{
				return currentLanguageData.dict[tid];
			}
			return tid;
		}

		/// <summary>
		/// Parse json data into the target object.
		/// Loads the dictionary as well.
		/// </summary>
		/// <param name="target">Target LangData object.</param>
		/// <param name="jsonData">Json data to be parsed.</param>
		private void ParseLangData(ref LangData target, SimpleJSON.JSONNode jsonData)
		{
			// Parse basic data
			if (jsonData.HasKey("isoCode"))
			{
				target.isoCode = jsonData["isoCode"];
			}

			if (jsonData.HasKey("fontName"))
			{
				target.fontName = jsonData["fontName"];
			}

			// Parse dictionary
			TextAsset txt = Resources.Load<TextAsset>(DataFolder + target.isoCode);
			if (txt != null)
			{
				string[] lines = txt.text.Split('\n');
				char[] separator = { '=' };
				for (int i = 0; i < lines.Length; i++)
				{
					// Parse line: make sure it has the exact expected format (key=value)
					string[] split = lines[i].Split(separator, 2, System.StringSplitOptions.RemoveEmptyEntries);
					if (split.Length == 2)
					{
						// Remove spaces at the end of the line for both keys and values
						string key = split[0].Trim();
						string value = split[1].Trim().Replace("\\n", "\n");    // Replace custom \n characters

						target.dict[key] = value;
					}
				}
			}
		}
		#endregion
	}
}
