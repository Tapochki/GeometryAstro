namespace Studio.Settings
{
    public enum SceneNames
    {
        Unknown,

        Splash,
        Game,
        Loading,
    }

    public enum GameStates
    {
        Unknown,

        Splash,
        Game,
        Loading,
        Menu,
    }

    public enum CacheType
    {
        AppSettingsData,
    }

    public enum PurchasingType
    {
        Unknown = -1,

        RemoveAds = 0,
    }

    public enum Languages // value of enum you can see in SystemLanguage class
    {
        Ukrainian = 38,
        Russian = 30,
        English = 10,
    }

    public enum LogTypes
    {
        Unknown,

        Info,
        Warning,
        Error,
        Debug,
    }

    internal enum SpreadsheetDataType
    {
        Localization,
    }

    public enum Sounds
    {
        Unknown,
    }
}