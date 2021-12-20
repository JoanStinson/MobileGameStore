using UnityEngine;
using Zenject;
using static JGM.GameStore.Localization.LocalizationService;

namespace JGM.GameStore.Localization
{
    public class ChangeLanguage : MonoBehaviour
    {
        [Inject]
        private ILocalizationService _localizationService;

        public void ChangeLanguageToRandom()
        {
            var newLanguage = (Language)Random.Range(0, (int)Language.Count);
            _localizationService.SetLanguage(newLanguage);
        }
    }
}