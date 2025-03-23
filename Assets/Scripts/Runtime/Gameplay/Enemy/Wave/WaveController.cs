using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Services;
using UniRx;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class WaveController : IDisposable
    {
        private TickService _tickService;
        private LevelConfig _levelConfig;

        private IEnemySpawner _enemySpawner;

        private WaveData _currentWave;
        private Dictionary<WaveEvent, int> _eventExecutions = new();

        private CompositeDisposable _disposables = new();
        private CompositeDisposable _waveDisposables = new();

        private BoolReactiveProperty _isBossActive = new(false);

        public int CurrentWaveIndex { get; private set; }
        public IReadOnlyReactiveProperty<bool> IsBossActive => _isBossActive;

        [Inject]
        private void Construct(IEnemySpawner enemySpawner, GameConfig gameConfig, TickService tickService)
        {
            _enemySpawner = enemySpawner;
            _levelConfig = gameConfig.LevelsConfig;
            _tickService = tickService;
        }

        public void Init()
        {
            StartWaves();
        }

        private void StartWaves()
        {
            SetNewWave(0);
        }

        private void SetNewWave(int waveId)
        {
            _waveDisposables?.Dispose();
            _waveDisposables = new CompositeDisposable();
            _eventExecutions.Clear();

            CurrentWaveIndex = waveId;
            _currentWave = _levelConfig.GetWhaveById(waveId);
            _enemySpawner.StartWave(_currentWave.enemies);

            SpawnBossIfNeeded();
            StartEnemySpawning();
            StartWaveTimer();
            ProcessWaveEvents();
        }

        private void TrySpawnEnemy()
        {
            if (_enemySpawner.ActiveEnemyCount < _currentWave.minEnemies)
            {
                int maxSpawnable = _currentWave.minEnemies - _enemySpawner.ActiveEnemyCount;
                int spawnCount = UnityEngine.Random.Range(4, Math.Min(12, maxSpawnable + 1));
                SpawnEnemyGroup(spawnCount);
            }
            else
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemyGroup(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            _enemySpawner.SpawnEnemy();
        }

        private IDisposable _waveTimerDisposable;

        private void StartWaveTimer()
        {
            _waveTimerDisposable?.Dispose();

            _waveTimerDisposable = _tickService.RegisterTimer(
                interval: TimeSpan.FromSeconds(_currentWave.waveDuration),
                callback: CompleteWave,
                checkPause: true
            ).AddTo(_waveDisposables);
        }

        private IDisposable _spawnIntervalDisposable;

        private void StartEnemySpawning()
        {
            if (_currentWave.enemies.Length == 0) return;

            _spawnIntervalDisposable?.Dispose();

            _spawnIntervalDisposable = _tickService.RegisterInterval(
                interval: TimeSpan.FromSeconds(_currentWave.spawnInterval),
                callback: () =>
                {
                    if (!_isBossActive.Value)
                        TrySpawnEnemy();
                },
                checkPause: true
            ).AddTo(_waveDisposables);
        }

        private void ProcessWaveEvents()
        {
            foreach (var waveEvent in _currentWave.waveEvents)
            {
                Observable.Timer(TimeSpan.FromSeconds(waveEvent.activationTime))
                    .Where(_ => !_isBossActive.Value && CheckEventChance(waveEvent))
                    .Subscribe(_ => HandleWaveEvent(waveEvent))
                    .AddTo(_waveDisposables);
            }
        }

        private bool CheckEventChance(WaveEvent waveEvent)
        {
            return UnityEngine.Random.value <= waveEvent.eventChance / 100f;
        }

        private void HandleWaveEvent(WaveEvent waveEvent)
        {
            if (_eventExecutions.ContainsKey(waveEvent))
            {
                if (_eventExecutions[waveEvent] >= waveEvent.maxRepetitions) return;
                _eventExecutions[waveEvent]++;
            }
            else
            {
                _eventExecutions.Add(waveEvent, 1);
            }

            Debug.Log($"Event triggered: {waveEvent.eventType}");
        }

        private void SpawnBossIfNeeded()
        {

        }

        private void OnBossDefeated()
        {
            _isBossActive.Value = false;
            CompleteWave();
        }

        private void CompleteWave()
        {
            if (CurrentWaveIndex >= _levelConfig.MaxWavesCount - 1)
            {
                Debug.Log("All waves completed!");
                return;
            }

            SetNewWave(CurrentWaveIndex + 1);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _waveDisposables?.Dispose();
        }
    }
}