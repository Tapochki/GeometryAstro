using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using UniRx;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class WaveController : IDisposable
    {
        private LevelConfig _levelConfig;
        private IEnemySpawner _enemySpawner;

        private WaveData _currentWave;
        private CompositeDisposable _disposables = new();
        private CompositeDisposable _waveDisposables = new();

        private BoolReactiveProperty _isBossActive = new BoolReactiveProperty(false);
        private Dictionary<WaveEvent, int> _eventExecutions = new Dictionary<WaveEvent, int>();

        public int CurrentWaveIndex { get; private set; }
        public IReadOnlyReactiveProperty<bool> IsBossActive => _isBossActive;

        [Inject]
        private void Construct(IEnemySpawner enemySpawner, GameConfig gameConfig)
        {
            _enemySpawner = enemySpawner;
            _levelConfig = gameConfig.LevelsConfig;
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

            SpawnBossIfNeeded();
            StartEnemySpawning();
            StartWaveTimer();
            ProcessWaveEvents();
        }

        private void StartEnemySpawning()
        {
            if (_currentWave.enemies.Length == 0) return;

            Observable.Interval(TimeSpan.FromSeconds(_currentWave.spawnInterval))
                .Where(_ => !_isBossActive.Value)
                .Subscribe(_ => SpawnRandomEnemy())
                .AddTo(_waveDisposables);
        }

        private void SpawnRandomEnemy()
        {
            var randomEnemy = _currentWave.enemies[UnityEngine.Random.Range(0, _currentWave.enemies.Length)];
            _enemySpawner.SpawnEnemy();
        }

        private void StartWaveTimer()
        {
            Observable.Timer(TimeSpan.FromSeconds(_currentWave.waveDuration))
                .Where(_ => !_isBossActive.Value)
                .Subscribe(_ => CompleteWave())
                .AddTo(_waveDisposables);
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
            //if (_currentWave.bossInPhase == null) return;

            //_isBossActive.Value = true;
            //_enemySpawner.SpawnBoss(_currentWave.bossInPhase.boss, OnBossDefeated);
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