using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TandC.Data;
using TandC.Settings;
using TandC.Utilities;
using UnityEngine;
using Zenject;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace TandC.Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        private const int ENEMY_PRELOAD_COUNT = 200;
        [SerializeField] private GameplayData _gameplayData;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemyParent;

        private Player _player;
        private EnemyFactory _enemyFactory;
        private EnemyDeathProcessor _enemyDeathProcessor;
        private EnemySpawnPositionService _enemySpawnPositionRegistrator;
        private ObjectPool<Enemy> _enemyPool;
        private List<EnemySpawnData> _currentWaveEnemies;
        private float _spawnDelay;
        private bool _isCanSpawn;

        [Inject]
        private void Construct(EnemyFactory enemyFactory, EnemySpawnPositionService enemySpawnPositionRegistrator, EnemyDeathProcessor enemyDeathProcessor, Player player)
        {
            _player = player;
            _enemyFactory = enemyFactory;
            _enemySpawnPositionRegistrator = enemySpawnPositionRegistrator;
            _enemyDeathProcessor = enemyDeathProcessor;
        }

        public void Start()
        {
            _enemyPool = new ObjectPool<Enemy>(Preload, GetReadyEnemy, BackEnemyToPool, ENEMY_PRELOAD_COUNT);
            _currentWaveEnemies = new List<EnemySpawnData>();
        }

        private List<Transform> GetSpawnPosition(SpawnType spawnType)
        {
            return _enemySpawnPositionRegistrator.GetSpawnPointsForType(spawnType);
        }

        private Vector2 GetDirectionPosition(TargetType targetType, Vector2 spawnPosition)
        {
            if(targetType == TargetType.Player) 
            {
                return _player.transform.position;
            }
            else 
            {
                return _enemySpawnPositionRegistrator.GetOppositePosition(spawnPosition);
            }
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
            return _gameplayData.GetEnemiesByType(enemyType);
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

        private Enemy Preload() => Instantiate(_enemyPrefab, _enemyParent);

        private void GetReadyEnemy(Enemy enemy){}

        private void BackEnemyToPool(Enemy enemy, bool isInitializeReturn)
        {
            if(!isInitializeReturn)
                _enemyDeathProcessor.EnemyDeathHandler(enemy);
            enemy.gameObject.SetActive(false);
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
                Vector2 directionPosition = GetDirectionPosition(selectedEnemySpawnData.targetType, spawnPoint.position);
                ConstractEnemy(currentEnemy, enemyData, spawnPoint.position, directionPosition);
            }
        }

        private void ConstractEnemy(Enemy enemy, EnemyData enemyData, Vector2 spawnPosition, Vector2 directPosition)
        {
            enemy.transform.position = spawnPosition;
            enemy = _enemyFactory.CreateEnemy(enemyData, enemy, BackEnemyToPool, _player.transform, directPosition, enemyData.BuilderType);
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

