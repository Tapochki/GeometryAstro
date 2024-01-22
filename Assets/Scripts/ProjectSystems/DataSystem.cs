using ChebDoorStudio.ScriptableObjects;
using ChebDoorStudio.Settings;
using ChebDoorStudio.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.ProjectSystems
{
    public class DataSystem : IInitializable
    {
        public event Action OnCacheLoadedEvent;

        public event Action<CacheType> OnCacheResetEvent;

        private Dictionary<CacheType, string> _cacheDataPathes;

        public PlayerVaultData PlayerVaultData { get; private set; }
        public AppSettingsData AppSettingsData { get; private set; }
        public PurchaseData PurchaseData { get; private set; }
        public ShopPurchasedItemData ShopPurchasedItemData { get; private set; }
        public SelectedPlayerSkinData SelectedPlayerSkinData { get; private set; }

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

                case CacheType.PurchaseData:
                    File.WriteAllText(_cacheDataPathes[type], InternalTools.SerializeData(PurchaseData));
                    break;

                case CacheType.PlayerValutData:
                    File.WriteAllText(_cacheDataPathes[type], InternalTools.SerializeData(PlayerVaultData));
                    break;

                case CacheType.ShopPurchasedItemData:
                    File.WriteAllText(_cacheDataPathes[type], InternalTools.SerializeData(ShopPurchasedItemData));
                    break;

                case CacheType.SelectedPlayerSkinData:
                    File.WriteAllText(_cacheDataPathes[type], InternalTools.SerializeData(SelectedPlayerSkinData));
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

                case CacheType.PurchaseData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        PurchaseData = new PurchaseData()
                        {
                            isRemovedAds = false,
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        PurchaseData = InternalTools.DeserializeData<PurchaseData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.PlayerValutData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        PlayerVaultData = new PlayerVaultData()
                        {
                            bestScore = 0,
                            coins = 0,
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        PlayerVaultData = InternalTools.DeserializeData<PlayerVaultData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.ShopPurchasedItemData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        ShopPurchasedItemData = new ShopPurchasedItemData()
                        {
                            shopItemData = new List<ShopItemType>()
                            {
                                ShopItemType.RedPlayer,
                            },
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        ShopPurchasedItemData = InternalTools.DeserializeData<ShopPurchasedItemData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.SelectedPlayerSkinData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        SelectedPlayerSkinData = new SelectedPlayerSkinData()
                        {
                            skin = ShopItemType.RedPlayer,
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        SelectedPlayerSkinData = InternalTools.DeserializeData<SelectedPlayerSkinData>(File.ReadAllText(_cacheDataPathes[type]));
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
                { CacheType.PurchaseData, Application.persistentDataPath + AppConstants.LOCAL_PURCHASE_DATA_FILE_PATH },
                { CacheType.PlayerValutData, Application.persistentDataPath + AppConstants.LOCAL_PLAYER_VAULT_DATA_FILE_PATH },
                { CacheType.ShopPurchasedItemData, Application.persistentDataPath + AppConstants.LOCAL_SHOP_DATA_FILE_PATH },
                { CacheType.SelectedPlayerSkinData, Application.persistentDataPath + AppConstants.LOCAL_SKIN_DATA_FILE_PATH },
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

                case CacheType.PurchaseData:

                    PurchaseData = new PurchaseData()
                    {
                        isRemovedAds = false,
                    };

                    break;

                case CacheType.PlayerValutData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        PlayerVaultData = new PlayerVaultData()
                        {
                            bestScore = 0,
                            coins = 0,
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        PlayerVaultData = InternalTools.DeserializeData<PlayerVaultData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.ShopPurchasedItemData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        ShopPurchasedItemData = new ShopPurchasedItemData()
                        {
                            shopItemData = new List<ShopItemType>()
                            {
                                ShopItemType.RedPlayer,
                            },
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        ShopPurchasedItemData = InternalTools.DeserializeData<ShopPurchasedItemData>(File.ReadAllText(_cacheDataPathes[type]));
                    }
                    break;

                case CacheType.SelectedPlayerSkinData:
                    if (!File.Exists(_cacheDataPathes[type]))
                    {
                        SelectedPlayerSkinData = new SelectedPlayerSkinData()
                        {
                            skin = ShopItemType.RedPlayer,
                        };

                        SaveCache(type);
                    }
                    else
                    {
                        SelectedPlayerSkinData = InternalTools.DeserializeData<SelectedPlayerSkinData>(File.ReadAllText(_cacheDataPathes[type]));
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