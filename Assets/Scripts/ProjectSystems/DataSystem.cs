using Studio.ScriptableObjects;
using Studio.Settings;
using Studio.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace Studio.ProjectSystems
{
    public class DataSystem : IInitializable
    {
        public event Action OnCacheLoadedEvent;

        public event Action<CacheType> OnCacheResetEvent;

        private Dictionary<CacheType, string> _cacheDataPathes;
        public AppSettingsData AppSettingsData { get; private set; }

        [Inject]
        public void Construct()
        {
            Utilities.Logger.Log("DataSystem Construct", LogTypes.Info);
        }

        public void Initialize()
        {
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
                    File.WriteAllText(_cacheDataPathes[type], InternalTools.SerializeData(AppSettingsData));
                    break;

                default:
                    Utilities.Logger.Log($"[{type}] is not implemented", LogTypes.Warning);
                    break;
            }
        }

        private void LoadCachedData(CacheType type)
        {
            switch (type)
            {
                case CacheType.AppSettingsData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        AppSettingsData = new AppSettingsData()
                        {
                            isFirstRun = true,
                            appLanguage = (Languages)Application.systemLanguage,
                            musicVolume = 1,
                            soundVolume = 1,
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        AppSettingsData = InternalTools.DeserializeData<AppSettingsData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;
                default:
                    {
                        Utilities.Logger.Log($"[{type}] is not implemented", LogTypes.Warning);
                        return;
                    }
            }
        }

        private void FillCacheDataPathes()
        {
            _cacheDataPathes = new Dictionary<CacheType, string>
            {
                { CacheType.AppSettingsData, Application.persistentDataPath + AppConstants.LOCAL_APP_DATA_FILE_PATH },
            };
        }

        public void ResetData(CacheType type)
        {
            switch (type)
            {
                case CacheType.AppSettingsData:

                    AppSettingsData = new AppSettingsData()
                    {
                        isFirstRun = true,
                        appLanguage = (Languages)Application.systemLanguage,
                        musicVolume = 1,
                        soundVolume = 1,
                    };

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