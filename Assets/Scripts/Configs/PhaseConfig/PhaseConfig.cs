using System;
using System.Collections.Generic;
using TandC.Settings;
using UnityEngine;

namespace TandC.Data
{
    [CreateAssetMenu(fileName = "PhaseConfig", menuName = "TandC/Game/PhaseConfig", order = 1)]
    public class PhaseConfig : ScriptableObject
    {
        [SerializeField] private List<Phase> _gamePhases;

        public int PhasesCount { get { return _gamePhases.Count; } }

        public Phase GetPhaseById(int id)
        {
            foreach (var item in _gamePhases)
            {
                if (item.PhaseId == id)
                {
                    return item;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class Phase
    {
        public int PhaseId;
        public float waveTime;
        public float enemySpawnDelay;

        public EnemySpawnData[] enemyInPhase;
    }

    [Serializable]
    public class EnemySpawnData
    {
        public EnemyType enemyType;
        public SpawnType spawnType;
        public TargetType targetType;
    }
}