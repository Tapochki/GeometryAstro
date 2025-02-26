using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class EnemySpawner : IEnemySpawner
    {
        private const int ENEMY_PRELOAD_COUNT = 300;

        private int _maximumEnemies = ENEMY_PRELOAD_COUNT;

        private Enemy _enemyPrefab;
        private GameObject _enemyParent;

        private LoadObjectsService _loadObjectsService;
        private Player _player;
        private EnemyConfig _enemiesConfig;
        private IEnemyFactory _enemyFactory;
        private IEnemySpawnPositionService _enemySpawnPositionService;
        private ObjectPool<Enemy> _enemyPool;

        private List<Enemy> _activeEnemyList;
        private List<EnemySpawnData> _currentWaveEnemies;

        public int ActiveEnemyCount => _activeEnemyList.Count;

        [Inject]
        private void Construct(
            LoadObjectsService loadObjectsService,
            IEnemySpawnPositionService enemySpawnPositionService,
            GameConfig gameConfig,
            Player player)
        {
            _loadObjectsService = loadObjectsService;
            _player = player;
            _enemySpawnPositionService = enemySpawnPositionService;
           // _enemyDeathProcessor = enemyDeathProcessor;
            _enemiesConfig = gameConfig.EnemyConfig;
        }

        public void Init()
        {
            _enemyParent = new GameObject("[EnemyParent]");

            _enemyPrefab = _loadObjectsService.GetObjectByPath<Enemy>("Prefabs/Gameplay/Enemies/BasicEnemy");

            if (_enemyPrefab == null)
            {
                Debug.LogError("Enemy prefab not found at path: Resources/Prefabs/Gameplay/Enemies/BasicEnemy");
            }

            _activeEnemyList = new List<Enemy>();
            _enemyFactory = new EnemyFactory();
            InitializePool();
            _currentWaveEnemies = new List<EnemySpawnData>();
        }

        private void InitializePool()
        {
            _enemyPool = new ObjectPool<Enemy>(
                preloadFunc: PreloadEnemy,
                getAction: EnableEnemy,
                returnAction: DisableEnemy,
                preloadCount: ENEMY_PRELOAD_COUNT
            );
        }

        public void StartWave(EnemySpawnData[] enemyDatas)
        {
            _currentWaveEnemies.Clear();
            _currentWaveEnemies.AddRange(enemyDatas);
        }

        public void SpawnEnemy()
        {
            if (_currentWaveEnemies.Count == 0)
            {
                Debug.LogWarning("No enemies available in current wave");
                return;
            }

            if(ActiveEnemyCount >= _maximumEnemies) 
            {
                Debug.LogWarning("Maximum enemies");
                return;
            }

            var spawnData = GetRandomEnemyInWaveData();
            var enemyData = GetEnemyData(spawnData.enemyType);
            var spawnPoint = GetSpawnPoint();

            if(spawnPoint != null) 
            {
                var enemy = _enemyPool.Get();
                _activeEnemyList.Add(enemy);
                SetupEnemy(enemy, enemyData, spawnPoint.position);
            }
        }

        private EnemySpawnData GetRandomEnemyInWaveData()
        {
            return _currentWaveEnemies[UnityEngine.Random.Range(0, _currentWaveEnemies.Count)];
        }

        private EnemyData GetEnemyData(EnemyType type)
        {
            return _enemiesConfig.GetEnemiesByType(type);
        }

        private Transform GetSpawnPoint()
        {
            return _enemySpawnPositionService.GetRandomPositionFromRegister();
        }

        private Enemy PreloadEnemy()
        {
            return MonoBehaviour.Instantiate(_enemyPrefab, _enemyParent.transform);
        }

        private void EnableEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private void DisableEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void SetupEnemy(Enemy enemy, EnemyData data, Vector2 spawnPosition)
        {
            var direction = _enemySpawnPositionService.GetOppositePosition(spawnPosition);
            enemy = _enemyFactory.CreateEnemy(
                enemyData: data,
                enemy: enemy,
                onDeathEvent: (enemy) => HandleEnemyDeath(enemy),
                target: _player.transform,
                moveDirection: direction,
                builderType: data.BuilderType
            );
            enemy.transform.position = spawnPosition;
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            _activeEnemyList.Remove(enemy);
            _enemyPool.Return(enemy);
        }

        public void SpawnBoss(EnemySpawnData bossData, Action onBossDefeated)
        {
            //Boss spawn
        }
    }
}