using JGM.GameStore.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JGM.GameStore.Localization
{
    public sealed partial class LocalizationService : ILocalizationService
    {
        public class LanguageChangedEvent : UnityEvent<Language, Language> { }
        public LanguageChangedEvent OnLanguageChanged { get; set; } = new LanguageChangedEvent();

        public Language CurrentLanguage { get; private set; } = Language.Count;

        private const Language _defaultLanguage = Language.English;
        private const string _dataFolder = "Localization/";
        private const string _configFilePath = "Data/localization_data";

        private LanguageData _currentLanguageData => _languages[CurrentLanguage];
        private Dictionary<Language, LanguageData> _languages = null;

        public LocalizationService()
        {
            var localizationText = Resources.Load<TextAsset>(_configFilePath);
            var localizationJson = JSONNode.Parse(localizationText.text);

            _languages = new Dictionary<Language, LanguageData>();
            for (Language language = Language.English; language < Language.Count; ++language)
            {
                var newLanguageData = new LanguageData();
                newLanguageData.Language = language;
                _languages[language] = newLanguageData;

                string langKey = language.ToString().ToLowerInvariant();
                if (localizationJson.HasKey(langKey))
                {
                    ParseLanguageData(ref newLanguageData, localizationJson[langKey]);
                }
            }

            SetLanguage(_defaultLanguage);
        }

        public void SetLanguage(Language language)
        {
            if (language == Language.Count)
            {
                return;
            }

            var previousLanguage = CurrentLanguage;
            CurrentLanguage = language;
            OnLanguageChanged?.Invoke(previousLanguage, CurrentLanguage);
        }

        public string Localize(string textId)
        {
            if (_currentLanguageData.Library.ContainsKey(textId))
            {
                return _currentLanguageData.Library[textId];
            }

            return textId;
        }

        public string GetFontNameForLanguage(Language language)
        {
            if (!_languages.ContainsKey(language))
            {
                return null;
            }

            return _languages[language].FontName;
        }

        private void ParseLanguageData(ref LanguageData languageData, JSONNode jsonData)
        {
            if (jsonData.HasKey("isoCode"))
            {
                languageData.IsoCode = jsonData["isoCode"];
            }

            if (jsonData.HasKey("fontName"))
            {
                languageData.FontName = jsonData["fontName"];
            }

            var languageText = Resources.Load<TextAsset>(_dataFolder + languageData.IsoCode);
            if (languageText != null)
            {
                string[] lines = languageText.text.Split('\n');
                char[] separator = { '=' };
                for (int i = 0; i < lines.Length; ++i)
                {
                    // Parse line: make sure it has the exact expected format (key=value)
                    string[] split = lines[i].Split(separator, 2, System.StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length == 2)
                    {
                        // Remove spaces at the end of the line for both keys and values
                        string key = split[0].Trim();
                        string value = split[1].Trim().Replace("\\n", "\n");
                        languageData.Library[key] = value;
                    }
                }
            }
        }
    }
}