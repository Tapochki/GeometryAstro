using Studio.Settings;

namespace Studio.Models
{
    public class CachedUserData
    {
        // coins
        // diamonds
        // chapter progress
        // etc
    }

    public class PurchaseData
    {
        public bool isRemovedAds;
    }

    public class AppSettingsData
    {
        public bool isFirstRun;
        public float soundVolume;
        public float musicVolume;
        public Languages appLanguage;
    }

    public class LocalisationSheetData
    {
        public string Key;
        public string English;
        public string Ukrainian;
        public string Russian;
    }
}