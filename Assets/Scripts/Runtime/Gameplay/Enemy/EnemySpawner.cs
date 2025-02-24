using System.Collections;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

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
        private IEnemySpawnPositionService _enemySpawnPositionRegistrator;
        private ObjectPool<Enemy> _enemyPool;
        private List<EnemySpawnData> _currentWaveEnemies;
        private float _spawnDelay;
        private bool _isCanSpawn;

        private void Construct(IEnemyFactory enemyFactory, IEnemySpawnPositionService enemySpawnPositionRegistrator, 
            EnemyConfig enemiesConfig, IEnemyDeathProcessor enemyDeathProcessor, Player player)
        {
            _player = player;
            _enemyFactory = enemyFactory;
            _enemySpawnPositionRegistrator = enemySpawnPositionRegistrator;
            _enemyDeathProcessor = enemyDeathProcessor;
            _enemiesConfig = enemiesConfig;
        }

        private void Start()
        {
            _enemyPool = new ObjectPool<Enemy>(Preload, GetReadyEnemy, BackEnemyToPool, ENEMY_PRELOAD_COUNT);
            _currentWaveEnemies = new List<EnemySpawnData>();
        }

        private List<Transform> GetSpawnPosition(SpawnType spawnType)
        {
            return _enemySpawnPositionRegistrator.GetSpawnPointsForType(spawnType);
        }

        private Vector2 GetDirectionPosition(Vector2 spawnPosition)
        {
            return _enemySpawnPositionRegistrator.GetOppositePosition(spawnPosition);
        }

        public void StartWave(EnemySpawnData[] enemyDatas, float spawnDelay) 
        {
            _currentWaveEnemies = new List<EnemySpawnData>();
            _spawnDelay = spawnDelay;
            _currentWaveEnemies.AddRange(enemyDatas);
            _isCanSpawn = true;
            StartCoroutine(SpawnEnemiesRoutine());
        }

        private EnemyData GetEnemyData(EnemyType enemyType) 
        {
            return _enemiesConfig.GetEnemiesByType(enemyType);
        }

        private EnemySpawnData GetEnemyFromWave()
        {
            if (_currentWaveEnemies.Count == 0)
            {
                Debug.LogError("No enemies in the current wave.");
                return null;
            }

            int randomIndex = Random.Range(0, _currentWaveEnemies.Count);
            return _currentWaveEnemies[randomIndex];
        }

        private Enemy Preload() => GameObject.Instantiate(_enemyPrefab, _enemyParent);

        private void GetReadyEnemy(Enemy enemy){}

        private void BackEnemyToPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void EnemyDeathProcess(Enemy enemy) 
        {
            _enemyDeathProcessor.EnemyDeathHandler(enemy);
            _enemyPool.Return(enemy);
        }

        private void SpawnEnemy() 
        {
            if (_currentWaveEnemies.Count == 0)
            {
                Debug.LogError("No enemies in the list.");
                return;
            }
            EnemySpawnData selectedEnemySpawnData = GetEnemyFromWave();
            Enemy currentEnemy = _enemyPool.Get();
            EnemyData enemyData = GetEnemyData(selectedEnemySpawnData.enemyType);
            List<Transform> spawnPoints = GetSpawnPosition(selectedEnemySpawnData.spawnType);

            foreach(var spawnPoint in spawnPoints) 
            {
                Vector2 directionPosition = GetDirectionPosition(spawnPoint.position);
                ConstractEnemy(currentEnemy, enemyData, spawnPoint.position, directionPosition);
            }
        }

        private void ConstractEnemy(Enemy enemy, EnemyData enemyData, Vector2 spawnPosition, Vector2 directPosition)
        {
            enemy.transform.position = spawnPosition;
            enemy = _enemyFactory.CreateEnemy(enemyData, enemy, EnemyDeathProcess, _player.transform, directPosition, enemyData.BuilderType);
            enemy.gameObject.SetActive(true);
        }

        private IEnumerator SpawnEnemiesRoutine()
        {
            while (_isCanSpawn)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}

