namespace ChebDoorStudio.Settings
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

    public enum ShopItemType
    {
        RedPlayer,
        GreenPlayer,
        BluePlayer,
        PinkPlayer,
        YellowPlayer,
        CyanPlayer,
        OrangePlayer,
        PurplePlayer,
    }

    public enum ItemTypes
    {
        Coin,
    }

    public enum CacheType
    {
        AppSettingsData,
        PurchaseData,
        PlayerValutData,
        ShopPurchasedItemData,
        SelectedPlayerSkinData,
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

    public enum Sounds
    {
        Unknown,

        Click,
        CoinPickUp,
        PlayerChangeDirection,
        PlayerDeath,
        Background,
    }
}