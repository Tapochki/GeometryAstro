using System;
using System.Collections.Generic;
using System.IO;
using TandC.Data;
using TandC.Settings;
using TandC.Utilities;
using UnityEngine;
using Zenject;

namespace TandC.ProjectSystems
{
    public class DataSystem : MonoBehaviour
    {
        public event Action OnCacheLoadedEvent;

        public event Action<CacheType> OnCacheResetEvent;

        private Dictionary<CacheType, string> _cacheDataPathes;

        public PlayerVaultData PlayerVaultData { get; private set; }
        public AppSettingsData AppSettingsData { get; private set; }
        public PurchaseData PurchaseData { get; private set; }
        public PlayerConfig PlayerConfig { get; private set; }

        [Inject]
        private void Construct(PlayerConfig playerConfig)
        {
            Utilities.Logger.Log("DataSystem Construct", LogTypes.Info);
            PlayerConfig = playerConfig;
        }

        public void Initialize()
        {
            Debug.LogError("Initi");

            FillCacheDataPathes();

            if (!Directory.Exists(AppConstants.PATH_TO_GAMES_CACHE))
            {
                Directory.CreateDirectory(AppConstants.PATH_TO_GAMES_CACHE);
            }

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

                case CacheType.PlayerData:
                    WriteTextToFile(_cacheDataPathes[type], InternalTools.SerializeData(PlayerConfig.PlayerData));
                    break;

                default:
                    Utilities.Logger.Log($"[{type}] is not implemented", LogTypes.Warning);
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

                case CacheType.PlayerData:
                    if (CheckIfPathExist(type, PlayerConfig.SetDefaultPlayerData)) 
                    {
                        PlayerConfig.PlayerData = InternalTools.DeserializeData<PlayerData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                default:
                    {
                        Utilities.Logger.Log($"[{type}] is not implemented", LogTypes.Warning);
                        return;
                    }
            }
        }

        private bool CheckIfPathExist(CacheType type, Action SetDefault) 
        {
            if (!File.Exists(_cacheDataPathes[type])) 
            {
                Debug.LogError(type);
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
                { CacheType.PlayerData,  Application.persistentDataPath + AppConstants.LOCAL_PLAYER_DATA_FILE_PATH}
            };
        }

        public void ResetData(CacheType type)
        {
            switch (type)
            {
                case CacheType.AppSettingsData:
                    SetDefaultPurchaseData();
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

                default:
                    {
                        Utilities.Logger.Log($"[{type}] is not implemented", LogTypes.Warning);
                        return;
                    }
            }

            SaveCache(type);
            OnCacheResetEvent?.Invoke(type);
        }
    }
}