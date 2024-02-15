using System;
using TandC.Settings;
using UnityEngine;

namespace TandC.Data
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "TandC/Game/GameplayData", order = 3)]
    public class GameplayData : ScriptableObject
    {  
        public PlayerWeaponData[] weaponData;
        public EnemyWeaponData[] enemyWeaponData;
        public PlayerData playerData;
        public PlayerBulletData[] bulletData;
        public EnemyBulletData[] enemyBulletData;
        public ItemData[] itemDatas;
        public MiniBoss[] miniBosses;
        public SkillType[] StartenSkills;
        public SkillType[] StartenSkills2;
        public DropChanceData DropChance;
        public BonusTypes[] bonusTypes;
        public Materials[] materials;

        public BonusTypes GetBonusByType(BonusType type)
        {
            foreach (var item in bonusTypes)
            {
                if (item.type == type)
                {
                    return item;
                }
            }

            return null;
        }

        public Materials GetMaterialByType(MaterialTypes type)
        {
            foreach (var item in materials)
            {
                if (item.type == type)
                {
                    return item;
                }
            }

            return null;
        }

        public MiniBoss GetMiniBossByPhaseId(int phaseId)
        {
            foreach (var item in miniBosses)
            {
                foreach (var bossPhases in item.PhaseID)
                {
                    if (phaseId == bossPhases)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public PlayerWeaponData GetWeaponByType(WeaponType weaponType)
        {
            foreach (var item in weaponData)
            {
                if (item.type == weaponType)
                {
                    return item;
                }
            }

            return null;
        }

        public EnemyWeaponData GetEnemyWeaponByType(EnemyType enemyType)
        {
            foreach (var item in enemyWeaponData)
            {
                if (item.type == enemyType)
                {
                    return item;
                }
            }

            return null;
        }

        public PlayerBulletData GetBulletByType(WeaponType weaponType)
        {
            foreach (var item in bulletData)
            {
                if (item.type == weaponType)
                {
                    return item;
                }
            }

            return null;
        }

        public EnemyBulletData GetEnemyBulletByType(EnemyType enemyType)
        {
            foreach (var item in enemyBulletData)
            {
                if (item.type == enemyType)
                {
                    return item;
                }
            }

            return null;
        }

        public ItemData GetItemDataByType(ItemType itemType)
        {
            foreach (var item in itemDatas)
            {
                if (item.type == itemType)
                {
                    return item;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class BonusTypes
    {
        public BonusType type;
        public Sprite sprite;
    }

    [Serializable]
    public class DropChanceData
    {
        public int StandartShotChance;
        public int LaserShotChance;
        public int RocketBlowChance;
        public int BombBlowChance;
        public int DashChance;
        public int DronChance;
    }

    [Serializable]
    public class DroneData
    {
        public GameObject prefab;
        public float StartDroneSpeed;
        public int StartDroneDamage;
    }

    public class BulletData
    {
        public int BulletSpeed;
        public float bulletLifeTime;
        public int bulletLife;
        public GameObject ButlletObject;
    }

    [Serializable]
    public class PlayerBulletData : BulletData
    {
        public WeaponType type;
        public bool IsNotCustomise;
        public CustomisationType CustomisationType;
    }

    [Serializable]
    public class EnemyBulletData : BulletData
    {
        public EnemyType type;
    }

    [Serializable]
    public class MiniBoss
    {
        public EnemyData enemyData;
        public int[] PhaseID;
    }

    [Serializable]
    public class Materials
    {
        public MaterialTypes type;
        public Material material;
    }

    [Serializable]
    public class ItemData
    {
        public int itemId;
        public int itemValueMin;
        public int itemValueMax;
        public float weight;
        public GameObject prefab;
        public Sprite sprite;
        public ItemType type;
        public bool isForBoss;

        [TextArea(5, 10)]
        public string description;
    }

    public class WeaponData
    {
        public string weaponName;
        public float baseDamage;
        public float shootDeley;
    }

    [Serializable]
    public class PlayerWeaponData : WeaponData
    {
        public WeaponType type;
    }

    [Serializable]
    public class EnemyWeaponData : WeaponData
    {
        public EnemyType type;
    }

   
}