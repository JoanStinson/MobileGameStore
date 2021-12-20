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

        [Inject]
        private ILocalizationService _localizationService;
        private TextMeshProUGUI _text;
        private string _stringBeforeKey = string.Empty;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _localizationService.OnLanguageChanged.AddListener(OnLanguageChanged);
            RefreshText();
        }

        private void OnLanguageChanged(Language languageKey, Language languageValue)
        {
            RefreshText();
        }

        public void RefreshText()
        {
            _text.text = $"{_stringBeforeKey}{_localizationService.Localize(_localizedKey)}";
        }

        public void RefreshText(in string localizedKey)
        {
            _localizedKey = localizedKey;
            RefreshText();
        }

        public void RefreshText(in string localizedKey, in string stringBeforeKey)
        {
            _localizedKey = localizedKey;
            _stringBeforeKey = stringBeforeKey;
            RefreshText();
        }
    }
}