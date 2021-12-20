using static JGM.GameStore.Localization.LocalizationService;

namespace JGM.GameStore.Localization
{
    public interface ILocalizationService
    {
        public LanguageChangedEvent OnLanguageChanged { get; set; }

        void SetLanguage(Language language);
        string Localize(string textId);
    }
}