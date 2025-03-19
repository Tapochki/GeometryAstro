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
        Health = 2,
        Armor = 3,
        SpeedMoving = 4,
        ReviveCount = 5,
        CriticalDamage = 6,
        CriticalChance = 7,
        CurseProvocation = 8,
        ÑurseReinforcment = 9,
        BulletsSize = 10,
        Duplicator = 11,
        HealtRestoreSpeed = 12,
        ReloadTimer = 13,
        PickUpRadius = 14,
        ReceivingExpirience = 15,
        ReceivingCoins = 16,
        // Active Skills
        //Guns Skills
        StandartGun = 17,
        AutoGun = 18,
        RocketGun = 19,
        LaserDestroyerGun = 20,
        MachineGun = 21,
        EnergyGun = 22,
        SawGun = 23,
        AuraGun = 24,
        NeedleGun = 25,
        SelfDirectedGun = 26,
        MineSpawnerGun = 27,

        // Possible weapons  Skills
        RifleGun = 28,
        FlamethrowerGun = 29,
        PushWaveGun = 30,
        ChainLightningGun = 31,
        IceRayGun = 32,

        //Drones Skills
        DronCaster = 33,
        Drone = 34,
        //Abilities Skills
        Dash = 35,
        Shield = 36,
        Cloaking = 37,
        //Possible Abilities Skills
        EnergyReleaser = 38,
        Overload = 39,
        ActiveShield = 40,

        //Inifinit Skills
        CoinInfitin = 41,
        ScoreInfinit = 42,
    }

    public enum ModificatorType 
    {
        None = 0,
        Damage = 1,
        Health = 2,
        Armor = 3,
        SpeedMoving = 4,
        ReviveCount = 5,
        CriticalDamage = 6,
        CriticalChance = 7,
        CurseProvocation = 8,
        ÑurseReinforcment = 9,
        BulletsSize = 10,
        Duplicator = 11,
        HealtRestoreSpeed = 12,
        ReloadTimer = 13,
        PickUpRadius = 14,
        ReceivingExpirience = 15,
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