using System.Collections.Generic;

namespace JGM.GameStore.Localization
{
    public partial class LocalizationService
    {
        public class LanguageData
        {
            public Language Language = Language.Count;
            public string IsoCode;
            public string FontName;
            public Dictionary<string, string> Library = new Dictionary<string, string>();
        }
    }
}