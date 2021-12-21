using JGM.GameStore.Loaders;
using TMPro;
using UnityEngine;
using Zenject;
using static JGM.GameStore.Localization.LocalizationService;

namespace JGM.GameStore.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField]
        private string _localizedKey = "INSERT_KEY_HERE";

        [Inject] private ILocalizationService _localizationService;
        [Inject] private IAssetsLibrary _assetsLibrary;

        private TextMeshProUGUI _text;
        private string _stringBeforeKey = string.Empty;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _localizationService.OnLanguageChanged.AddListener(OnLanguageChanged);
            string newLanguageFontName = _localizationService.GetFontNameForLanguage(_localizationService.CurrentLanguage);
            _text.font = _assetsLibrary.GetFontAsset(newLanguageFontName);
            RefreshText();
        }

        private void OnLanguageChanged(Language previousLanguage, Language newLanguage)
        {
            if (newLanguage == previousLanguage)
            {
                return;
            }

            string newLanguageFontName = _localizationService.GetFontNameForLanguage(newLanguage);
            _text.font = _assetsLibrary.GetFontAsset(newLanguageFontName);
            RefreshText();
        }

        public void RefreshText()
        {
            _text.text = $"{_stringBeforeKey}{_localizationService.Localize(_localizedKey)}";
        }

        public void RefreshText(in string localizedKey)
        {
            RefreshText(localizedKey, string.Empty);
        }

        public void RefreshText(in string localizedKey, in string stringBeforeKey)
        {
            _localizedKey = localizedKey;
            _stringBeforeKey = stringBeforeKey;
            RefreshText();
        }
    }
}