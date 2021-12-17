using static JGM.GameStore.Localization.LocalizationService;

namespace JGM.GameStore.Localization
{
    public interface ILocalizationService
    {
        void SetLanguage(Language language);
        string Localize(string textId);
    }
}