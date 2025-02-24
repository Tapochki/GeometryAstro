using TandC.GeometryAstro.Data;
using UniRx;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class WaveController
    {
        private LevelConfig _levelConfig;

        private IEnemySpawner _enemySpawner;
        private WaveData _currentWave;

        private CompositeDisposable _disposables = new();

        private float _cooldownToSpawnEnemy;

        public int CurrentWaveIndex { get; private set; }

        [Inject]
        private void Construct(IEnemySpawner enemySpawner, GameConfig gameConfig)
        {
            _enemySpawner = enemySpawner;
            _levelConfig = gameConfig.LevelsConfig;
        }

        public void Init()
        {
            StartWaves();
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(_disposables);
        }

        private void StartWaves()
        {
            SetNewPhase(0);
        }

        private void SetNewPhase(int phaseId)
        {
            CurrentWaveIndex = phaseId;
            _currentWave = _levelConfig.GetWhaveById(phaseId);
            _cooldownToSpawnEnemy = _currentWave.waveDuration;
            _enemySpawner.StartWave(_currentWave.enemies, _currentWave.spawnInterval);
        }

        private void IncreaseWaveIndex()
        {
            CurrentWaveIndex++;
            if (CurrentWaveIndex >= _levelConfig.WhavesCount - 1)
            {
                CurrentWaveIndex = 0;
            }
            SetNewPhase(CurrentWaveIndex);
        }

        private void Update()
        {
            _cooldownToSpawnEnemy -= Time.deltaTime;
            if (_cooldownToSpawnEnemy <= 0)
            {
                IncreaseWaveIndex();
            }
        }
    }
}