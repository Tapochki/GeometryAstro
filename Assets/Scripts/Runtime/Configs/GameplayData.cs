using System;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "TandC/Game/GameplayData", order = 3)]
    public class GameplayData : ScriptableObject
    {  
        public MiniBoss[] miniBosses;
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
   
}