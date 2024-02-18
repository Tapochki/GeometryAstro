using TandC.Data;
using UnityEngine;
using Zenject;

namespace TandC.Gameplay
{
    public class WaveController : MonoBehaviour
    {
        private PhaseConfig _phaseConfig;

        private IEnemySpawner _enemySpawner;
        private Phase _currentPhase;

        private float _cooldownToSpawnEnemy;

        public int CurrentPhaseIndex { get; private set; }

        [Inject]
        private void Construct(IEnemySpawner enemySpawner, PhaseConfig phaseConfig)
        {
            _enemySpawner = enemySpawner;
            _phaseConfig = phaseConfig;
        }

        private void Start()
        {
            StartWaves();
        }

        private void StartWaves()
        {
            SetNewPhase(0);
        }

        private void SetNewPhase(int phaseId)
        {
            //if (CurrentPhaseIndex > _phaseConfig.gamePhases.Length)
            //{
            //    IncreasePhaseIndex();
            //}
            CurrentPhaseIndex = phaseId;
            _currentPhase = _phaseConfig.GetPhaseById(phaseId);
            _enemySpawner.StartWave(_currentPhase.enemyInPhase, _currentPhase.enemySpawnDelay);
        }

        private void IncreasePhaseIndex()
        {
            CurrentPhaseIndex++;
            if (CurrentPhaseIndex >= _phaseConfig.PhasesCount - 1)
            {
                //TODO _enemySpawner.IncreaseEnemyParam();
                CurrentPhaseIndex = 0;
            }
            SetNewPhase(CurrentPhaseIndex);
        }

        private void Update()
        {
            _cooldownToSpawnEnemy -= Time.deltaTime;
            if (_cooldownToSpawnEnemy <= 0)
            {
                _cooldownToSpawnEnemy = _currentPhase.waveTime;
            }
        }
    }
}