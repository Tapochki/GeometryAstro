using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        private const int ENEMY_PRELOAD_COUNT = 200;

        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemyParent;

        private Player _player;
        private EnemyConfig _enemiesConfig;
        private IEnemyFactory _enemyFactory;
        private IEnemyDeathProcessor _enemyDeathProcessor;
        private IEnemySpawnPositionService _enemySpawnPositionService;
        private ObjectPool<Enemy> _enemyPool;
        private List<EnemySpawnData> _currentWaveEnemies;

        [Inject]
        private void Construct(
            IEnemyFactory enemyFactory,
            IEnemySpawnPositionService enemySpawnPositionService,
            EnemyConfig enemiesConfig,
            IEnemyDeathProcessor enemyDeathProcessor,
            Player player)
        {
            _player = player;
            _enemyFactory = enemyFactory;
            _enemySpawnPositionService = enemySpawnPositionService;
            _enemyDeathProcessor = enemyDeathProcessor;
            _enemiesConfig = enemiesConfig;
        }

        private void Start()
        {
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

            var spawnData = GetRandomSpawnData();
            var enemyData = GetEnemyData(spawnData.enemyType);
            var spawnPoints = GetSpawnPoints(spawnData.spawnType);

            foreach (var point in spawnPoints)
            {
                var enemy = _enemyPool.Get();
                SetupEnemy(enemy, enemyData, point.position);
            }
        }

        private EnemySpawnData GetRandomSpawnData()
        {
            return _currentWaveEnemies[UnityEngine.Random.Range(0, _currentWaveEnemies.Count)];
        }

        private EnemyData GetEnemyData(EnemyType type)
        {
            return _enemiesConfig.GetEnemiesByType(type);
        }

        private List<Transform> GetSpawnPoints(SpawnType spawnType)
        {
            return _enemySpawnPositionService.GetSpawnPointsForType(spawnType);
        }

        private Enemy PreloadEnemy()
        {
            return Instantiate(_enemyPrefab, _enemyParent);
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
            enemy.transform.position = spawnPosition;
            var direction = _enemySpawnPositionService.GetOppositePosition(spawnPosition);

            _enemyFactory.CreateEnemy(
                enemyData: data,
                enemy: enemy,
                onDeathEvent: (enemy) => HandleEnemyDeath(enemy),
                target: _player.transform,
                moveDirection: direction,
                builderType: data.BuilderType
            );
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            _enemyDeathProcessor.EnemyDeathHandler(enemy);
            _enemyPool.Return(enemy);
        }

        public void SpawnBoss(EnemySpawnData bossData, Action onBossDefeated)
        {
            //Boss spawn
        }
    }
}