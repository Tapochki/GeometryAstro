namespace TandC.GeometryAstro.Settings
{
    public enum SceneNames
    {
        Unknown,

        Splash,
        Game,
        Loading,
        Menu,
    }

    public enum BonusType
    {
        Damage,
        Freeze,
        BlowUp
    }

    public enum ItemType
    {
        SmallXp,
        MeduimXp,
        BigXp,
        Ammo,
        Medecine,
        FrozenBomb,
        RocketBox,
        Chest,
        Bomb,
        Magnet,
        SmallMoney
    }

    public enum ActiveButtonType
    {
        MaskButton,
        LaserButton,
        RocketButton,
        DashButton,
    }

    public enum UpgradeType
    {
        Health,
        Armor,
        Speed,
        DamageMultiplier,
        CriticalChance,
        CriticalDamageMultiplier,
        RecoverTimerMultiplier,
        MoneyMultiplier,
        PickUpRadius,
        StarterSkill,
    }

    public enum CustomisationType
    {
        Player = 0,
        DefaultBullet = 1,
        AutoBullet = 2,
        MiniGunBullet = 3,
        RocketBullet = 4,
        Drones = 5,
    }

    public enum EnemyBuilderType
    {
        Default,
        Saw,
    }

    public enum SpawnPositionType
    {
        CornerTopLeft,
        CornerTopRight,
        CornerBottomLeft,
        CornerBottomRight,
        HorizontalTop,
        HorizontalTopLeft,
        HorizontalTopCenter,
        HorizontalTopRight,
        HorizontalBottom,
        HorizontalBottomLeft,
        HorizontalBottomCenter,
        HorizontalBottomRight,
        VerticalLeftTop,
        VerticalLeftCenter,
        VerticalLeftBottom,
        VerticalRightTop,
        VerticalRightCenter,
        VerticalRightBottom
    }

    //public enum TargetType
    //{
    //    Player,
    //    RandomPosition,
    //    OpositionPosition,
    //}

    public enum DropItemRareType
    {
        DefaultDrop,
        OnlyExpirienceDrop,
        RareDrop,
        BossDrop,
    }

    public enum SpawnType
    {
        RandomSpawn,
        CircleSpawn,
        AllTopHorizontalSpawn,
        AllBottomHorizontalSpawn,
        AllRightVerticalSpawn,
        AllLeftVerticalSpawn,
        SpawnByLineOnOnePisition,
        SpawnOnPlayerDirectionm,
    }

    public enum MaterialTypes
    {
        DefaultEnemyMaterial,
        FlashEnemyMaterial
    }

    public enum WaveEventType 
    {
        none = 0,
    }

    public enum EnemyType
    {
        StandartSquare,
        StandartPentagon,
        Star,
        Mine,
        Saw,
        PiciesFull,
        PiciesHalf,
        PiciesSmall,
        MiniBoss,
        SmallSquare,
        ShootingRoundedBall,
        Impulse,
        ImpulseSaw,
    }

    public enum WeaponType
    {
        Undefined,
        StandardGun,
        RocketLauncher,
        AutoGun,
        LaserGun,
        Minigun,
        EnergyGun,
        LightningGun,
    }

    public enum SkillUseType
    {
        Active,
        Passive,
        //Additional,
    }

    public enum SkillType
    {
        StandartGun,
        MaxHealthIncrease,
        MovementSpeedIncrease,
        Shield,
        Armor,
        BlowMina,
        BulletSpeed,
        RecoverTimerDecrease,
        DamageIncrease,
        Drone,
        CriticalChanceIncrease,
        CriticalDamageMultilpier,
        AutoGun,
        Dash,
        Mask,
        Rocket,
        HealthRestore,
        XpMultiplierIncrese,
        LaserGun,
        Minigun,
        EnergyGun,
        LightningGun,
        PickUpRadiusIncrease,
    }

    public enum GameStates
    {
        Unknown,

        Splash,
        Game,
        Loading,
        Menu,
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
        UserData
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
        Background,
    }
}