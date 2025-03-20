using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using TandC.GeometryAstro.Utilities;
using TandC.GeometryAstro.Utilities.Logging;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Services
{
    public class DataService : ILoadUnit
    {
        public event Action OnCacheLoadedEvent;

        public event Action<CacheType> OnCacheResetEvent;

        private Dictionary<CacheType, string> _cacheDataPathes;

        public PlayerVaultData PlayerVaultData { get; private set; }
        public AppSettingsData AppSettingsData { get; private set; }
        public PurchaseData PurchaseData { get; private set; }
        public UserData UserData { get; private set; }
        public ModificatorUpgradeData ModificatorUpgrade { get; private set; }

        private ModificatorUpgradeConfig _modificatorConfig;

        [Inject]
        private void Construct(MenuConfig menuConfig)
        {
            _modificatorConfig = menuConfig.ModificatorConfig;
        }

        public async UniTask Load()
        {
            Initialize();
            await UniTask.CompletedTask;
        }

        private void Initialize()
        {
            FillCacheDataPathes();

            if (!Directory.Exists(AppConstants.PATH_TO_GAMES_CACHE))
            {
                Directory.CreateDirectory(AppConstants.PATH_TO_GAMES_CACHE);
            }

           // UserData = new UserData();

            StartLoadCache();
        }

        private void StartLoadCache()
        {
            for (int i = 0; i < Enum.GetNames(typeof(CacheType)).Length; i++)
            {
                LoadCachedData((CacheType)i);
            }

            OnCacheLoadedEvent?.Invoke();
        }

        public void SaveAllCache()
        {
            int count = Enum.GetNames(typeof(CacheType)).Length;
            for (int i = 0; i < count; i++)
            {
                SaveCache((CacheType)i);
            }
        }

        public void SaveCache(CacheType type)
        {
            if (!File.Exists(_cacheDataPathes[type]))
            {
                File.Create(_cacheDataPathes[type]).Close();
            }

            switch (type)
            {
                case CacheType.AppSettingsData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(AppSettingsData));
                    break;

                case CacheType.PurchaseData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(PurchaseData));
                    break;

                case CacheType.PlayerValutData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(PlayerVaultData));
                    break;

                case CacheType.UserData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(UserData));
                    break;

                case CacheType.UpgradeData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(ModificatorUpgrade));
                    break;

                default:
                    Log.Default.W($"[{type}] is not implemented");
                    break;
            }
        }

        private void WriteTextToFile(string dataPath, string contents)
        {
            File.WriteAllText(dataPath, contents);
        }

        private void SetDefaultAppSettingData()
        {
            AppSettingsData = new AppSettingsData()
            {
                isFirstRun = true,
                appLanguage = (Languages)Application.systemLanguage,
                musicVolume = 1,
                soundVolume = 1,
            };
        }

        private void SetDefaultPurchaseData()
        {
            PurchaseData = new PurchaseData()
            {
                isRemovedAds = false,
            };
        }

        private void SetDefaultPlayerVaultData()
        {
            PlayerVaultData = new PlayerVaultData()
            {
                bestScore = 0,
                coins = 0,
            };
        }

        private void SetDefaultUserData() 
        {
            UserData = new UserData();
        }

        private void SetDefaultUpgradeData() 
        {
            ModificatorUpgrade = new ModificatorUpgradeData();
            ModificatorUpgrade.UpgradeModificatorsData = new List<UpgradeData>();
            foreach (var data in _modificatorConfig.StartModificatorsData)
            {
                ModificatorUpgrade.UpgradeModificatorsData.Add(
                    new UpgradeData() { IncreamentData = data.IncreamentData, CurrentLevel = 0 }
                    );
            }
        }

        private void LoadCachedData(CacheType type)
        {
            switch (type)
            {
                case CacheType.AppSettingsData:
                    if (CheckIfPathExist(type, SetDefaultAppSettingData))
                    {
                        AppSettingsData = InternalTools.DeserializeData<AppSettingsData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.PurchaseData:
                    if (CheckIfPathExist(type, SetDefaultPurchaseData))
                    {
                        PurchaseData = InternalTools.DeserializeData<PurchaseData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.PlayerValutData:
                    if (CheckIfPathExist(type, SetDefaultPlayerVaultData))
                    {
                        PlayerVaultData = InternalTools.DeserializeData<PlayerVaultData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.UserData:
                    if (CheckIfPathExist(type, SetDefaultUserData))
                    {
                        UserData = InternalTools.DeserializeData<UserData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.UpgradeData:
                    if (CheckIfPathExist(type, SetDefaultUpgradeData))
                    {
                        ModificatorUpgrade = InternalTools.DeserializeData<ModificatorUpgradeData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                default:
                    {
                        Log.Default.W($"[{type}] is not implemented");
                        return;
                    }
            }
        }

        private bool CheckIfPathExist(CacheType type, Action SetDefault)
        {
            if (!File.Exists(_cacheDataPathes[type]))
            {
                SetDefault?.Invoke();
                SaveCache(type);
                return false;
            }
            return true;
        }

        private void FillCacheDataPathes()
        {
            _cacheDataPathes = new Dictionary<CacheType, string>
            {
                { CacheType.AppSettingsData, Application.persistentDataPath + AppConstants.LOCAL_APP_DATA_FILE_PATH },
                { CacheType.PurchaseData, Application.persistentDataPath + AppConstants.LOCAL_PURCHASE_DATA_FILE_PATH },
                { CacheType.PlayerValutData, Application.persistentDataPath + AppConstants.LOCAL_PLAYER_VAULT_DATA_FILE_PATH },
                { CacheType.UserData,  Application.persistentDataPath + AppConstants.LOCAL_PLAYER_DATA_FILE_PATH},
                { CacheType.UpgradeData,  Application.persistentDataPath + AppConstants.LOCAL_UPGRADE_DATA_FILE_PATH}
            };
        }

        public void ResetData(CacheType type)
        {
            switch (type)
            {
                case CacheType.AppSettingsData:
                    SetDefaultAppSettingData();
                    break;

                case CacheType.PurchaseData:
                    SetDefaultPurchaseData();
                    break;

                case CacheType.PlayerValutData:
                    if (CheckIfPathExist(type, SetDefaultPlayerVaultData))
                    {
                        PlayerVaultData = InternalTools.DeserializeData<PlayerVaultData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.UserData:
                    SetDefaultUserData();
                    break;

                case CacheType.UpgradeData:
                    SetDefaultUpgradeData();
                    break;

                default:
                    {
                        Log.Default.W($"[{type}] is not implemented");
                        return;
                    }
            }

            SaveCache(type);
            OnCacheResetEvent?.Invoke(type);
        }
    }
}