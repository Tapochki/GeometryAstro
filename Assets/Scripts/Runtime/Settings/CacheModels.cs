using System;
using System.Collections.Generic;

namespace TandC.GeometryAstro.Settings
{
    public class PlayerVaultData
    {
        public int bestScore;
        public int coins;
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

    public class ModificatorUpgradeData
    {
        public List<UpgradeData> UpgradeModificatorsData;
    }

    public class UpgradeData
    {
        public IncreamentModificatorData IncreamentData;

        public int CurrentLevel = 0;

        public float CurrentValue
        {
            get { return (IncreamentData.IncrementValue * CurrentLevel); }
        }

        public void IncreaseLevel()
        {
            CurrentLevel++;
        }
    }
    [Serializable]
    public class IncreamentModificatorData 
    {
        public ModificatorType Type;
        public bool IsPercentageValue;
        public float IncrementValue;
    }

    public class LocalisationSheetData
    {
        public string Key;
        public string English;
        public string Ukrainian;
        public string Russian;
    }
}