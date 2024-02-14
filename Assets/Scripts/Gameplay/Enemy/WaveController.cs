using TandC.Data;
using UnityEngine;
using Zenject;

namespace TandC.Gameplay
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField] private GameplayData _gameplayData;

        private IEnemySpawner _enemySpawner;

        private float _cooldownToSpawnEnemy;

        private Phase _currentPhase;
        public int CurrentPhaseIndex { get; private set; }

        [Inject]
        private void Construct(IEnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
        }

        public void Start()
        {
            StartWaves();
        }

        public void StartWaves()
        {
            SetNewPhase(0);
        }

        private void SetNewPhase(int phaseId)
        {
            //if (CurrentPhaseIndex > _gameplayData.gamePhases.Length)
            //{
            //    IncreasePhaseIndex();
            //}
            CurrentPhaseIndex = phaseId;
            _currentPhase = _gameplayData.GetPhaseById(phaseId);
            _enemySpawner.StartWave(_currentPhase.enemyInPhase, _currentPhase.enemySpawnDelay);
        }

        public void IncreasePhaseIndex()
        {
            CurrentPhaseIndex++;
            if (CurrentPhaseIndex >= _gameplayData.gamePhases.Length - 1)
            {
                //_enemySpawner.IncreaseEnemyParam();
                CurrentPhaseIndex = 0;
            }
            SetNewPhase(CurrentPhaseIndex);
        }

        public void Update()
        {
            _cooldownToSpawnEnemy -= Time.deltaTime;
            if (_cooldownToSpawnEnemy <= 0)
            {
                _cooldownToSpawnEnemy = _currentPhase.waveTime;
            }
        }
    }
}