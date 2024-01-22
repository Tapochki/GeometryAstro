using ChebDoorStudio.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace ChebDoorStudio.Settings
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

    public class LocalisationSheetData
    {
        public string Key;
        public string English;
        public string Ukrainian;
        public string Russian;
    }
}