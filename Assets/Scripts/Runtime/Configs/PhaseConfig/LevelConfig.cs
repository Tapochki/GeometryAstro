using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "TandC/Game/LevelConfig", order = 1)]
    public class LevelConfig : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string _levelName = "Level 1";
        [SerializeField][TextArea(3, 5)] private string _description = "Level description";
        [SerializeField] private Sprite _levelPreviewImage;
        [Header("Waves Configuration")]
        [SerializeField] private List<WaveData> _gameWaves;

        [Header("Difficulty Settings")]
        [SerializeField] private DifficultyLevel _baseDifficulty = DifficultyLevel.Normal;
        [SerializeField] private DifficultyMultipliers _difficultyMultipliers;

        public int MaxWavesCount => _gameWaves.Count;
        public string LevelName => _levelName;
        public Sprite LevelPreview => _levelPreviewImage;


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

        public float GetHealthMultiplier(DifficultyLevel difficulty)
        {
            return _difficultyMultipliers.GetMultiplierForDifficulty(difficulty).healthMultiplier;
        }

        public float GetDamageMultiplier(DifficultyLevel difficulty)
        {
            return _difficultyMultipliers.GetMultiplierForDifficulty(difficulty).damageMultiplier;
        }

        public float GetRewardMultiplier(DifficultyLevel difficulty)
        {
            return _difficultyMultipliers.GetMultiplierForDifficulty(difficulty).rewardMultiplier;
        }

        public float GetScoreMultiplier(DifficultyLevel difficulty)
        {
            return _difficultyMultipliers.GetMultiplierForDifficulty(difficulty).scoreMultiplier;
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
        public WaveEventType eventType;
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

    [System.Serializable]
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard,
        Nightmare
    }

    [System.Serializable]
    public struct DifficultyMultipliers
    {
        public DifficultySettings easy;
        public DifficultySettings normal;
        public DifficultySettings hard;
        public DifficultySettings nightmare;

        public DifficultySettings GetMultiplierForDifficulty(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Easy => easy,
                DifficultyLevel.Normal => normal,
                DifficultyLevel.Hard => hard,
                DifficultyLevel.Nightmare => nightmare,
                _ => normal
            };
        }
    }

    [System.Serializable]
    public struct DifficultySettings
    {
        [Range(0.5f, 3f)] public float healthMultiplier;
        [Range(0.5f, 3f)] public float damageMultiplier;
        [Range(0.5f, 3f)] public float rewardMultiplier;
        [Range(0.5f, 3f)] public float scoreMultiplier;
    }
}