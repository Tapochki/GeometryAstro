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
        RocketAmmo,
        Medecine,
        FrozenBomb,
        RocketBox,
        Chest,
        Bomb,
        Magnet,
        Coin
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
        OnlyOneThingDropForTest,
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
        RocketGun,
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
        //Passive Skills
        None = 0,
        Damage = 1,
        MaxHealth = 2,
        Armor = 3,
        SpeedMoving = 4,
        ReviveCount = 5,
        CriticalDamage = 6,
        CriticalChance = 7,
        CurseProvocation = 8,
        ÑurseReinforcment = 9,
        BulletsSize = 10,
        Duplicator = 11,
        HealtRestoreCount = 12,
        ReloadTimer = 13,
        PickUpRadius = 14,
        ReceivedExperience = 15,
        ReceivedCoins = 16,
        Luck = 17,
        // Active Skills
        //Guns Skills
        StandartGun = 18,
        AutoGun = 19,
        RocketGun = 20,
        LaserDestroyerGun = 21,
        MachineGun = 22,
        EnergyGun = 23,
        SawGun = 24,
        AuraGun = 25,
        NeedleGun = 26,
        SelfDirectedGun = 27,
        MineSpawnerGun = 28,

        // Possible weapons  Skills
        RifleGun = 29,
        FlamethrowerGun = 30,
        PushWaveGun = 31,
        ChainLightningGun = 32,
        IceRayGun = 33,

        //Drones Skills
        DronCaster = 34,
        Drone = 35,
        //Abilities Skills
        Dash = 36,
        Shield = 37,
        Cloaking = 38,
        //Possible Abilities Skills
        EnergyReleaser = 39,
        Overload = 40,
        ActiveShield = 41,

        //Inifinit Skills
        CoinInfinite = 42,
        ScoreInfinite = 43,
    }

    public enum ModificatorType 
    {
        None = 0,
        Damage = 1,
        MaxHealth = 2,
        Armor = 3,
        SpeedMoving = 4,
        ReviveCount = 5,
        CriticalDamageMultiplier = 6,
        CriticalChance = 7,
        CurseStrength = 8,
        CurseSpeed = 9,
        BulletsSize = 10,
        Duplicator = 11,
        HealtRestoreCount = 12,
        ReloadTimer = 13,
        PickUpRadius = 14,
        ReceivedExperience = 15,
        ReceivingCoins = 16,
        //Possible Passive Skill
        Luck = 17
    }

    public enum ActiveSkillType 
    {
        None = 0,
        //Guns
        StandartGun = 1,
        AutoGun = 2,
        RocketGun = 3,
        LaserDestroyerGun = 4,
        MachineGun = 5,
        EnergyGun = 6,
        SawGun = 7,
        AuraGun = 8,
        NeedleGun = 9,
        SelfDirectedGun = 10,
        MineSpawnerGun = 11,

        // Possible weapons 
        RifleGun = 12,
        FlamethrowerGun = 13,
        PushWaveGun = 14,
        ChainLightningGun = 15,
        IceRayGun = 16,

        //Drones
        DronCaster = 17,
        Drone = 18,
        //Abilities
        Dash = 19,
        Shield = 20,
        Cloaking = 21,
        //Possible Abilities
        EnergyReleaser = 22,
        Overload = 23,
        ActiveShield = 24
    }

    public enum SkillActivationType 
    {
        None,
        NewSkill,
        Evolution,
        UpgradeActive,
        UpgradePassive,
        InfinitSkill
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
        UserData,
        UpgradeData
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