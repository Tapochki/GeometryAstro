using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "TandC/Game/LevelConfig", order = 1)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<WaveData> _gameWaves;

        public int WhavesCount { get { return _gameWaves.Count; } }

        public WaveData GetWhaveById(int id)
        {
            foreach (var item in _gameWaves)
            {
                if (item.WaveId == id)
                {
                    return item;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class EnemySpawnData
    {
        public EnemyType enemyType;
        public SpawnType spawnType;
    }

    [Serializable]
    public class WaveEvent
    {
        public string eventName;
        [Range(0, 60)] public float activationTime;
        [Range(0, 100)] public float eventChance;
        public int maxRepetitions = 1;
    }

    [Serializable]
    public class WaveData
    {
        public int WaveId;
        [Header("Enemy Settings")]
        public EnemySpawnData[] enemies;
        public int minEnemies = 10;
        public float spawnInterval = 2f;

        [Header("Time Settings")]
        public float waveDuration = 60f;

        [Header("Events")]
        public WaveEvent[] waveEvents;

        [Header("Boss Settings")]
        public BossData bossInPhase;
    }

    [Serializable]
    public class BossData 
    {
        public EnemySpawnData boss;
    }
}