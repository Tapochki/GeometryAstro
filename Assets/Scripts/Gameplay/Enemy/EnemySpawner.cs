using System.Collections;
using System.Collections.Generic;
using TandC.Data;
using TandC.Settings;
using TandC.Utilities;
using UnityEditorInternal;
using UnityEngine;

namespace TandC.Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        private const int ENEMY_PRELOAD_COUNT = 200;
        [SerializeField] private GameplayData _gameplayData;
        [SerializeField] EnemyFactory _enemyFactory;
        [SerializeField] Enemy _enemyPrefab;
        [SerializeField] Camera _camera;
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private Player _player;

        private float _camHeight;
        private float _camWidth;

        private ObjectPool<Enemy> _enemyPool;
        private List<EnemySpawnData> _currentWaveEnemies;

        public void Start()
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            _camHeight = _camera.orthographicSize;
            _camWidth = screenAspect * _camHeight;
            _enemyPool = new ObjectPool<Enemy>(Preload, GetAction, ReturnAction, ENEMY_PRELOAD_COUNT);
            _currentWaveEnemies = new List<EnemySpawnData>();
        }

        private Vector2 GetSpawnPosition(SpawnType spawnType)
        {
            Vector2 position;
            Transform spawnTransform;
            if (spawnType == SpawnType.Random)
            {
                int index = UnityEngine.Random.Range(0, _spawnPositions.Count);

                spawnTransform = _spawnPositions[index];
            }
            Transform spawnPoint = _spawnPositions[(int)spawnType];

            if (spawnType == SpawnType.SpawnFrontPlayer)
            {
                spawnPoint.transform.localPosition = new Vector2(0, spawnPoint.transform.localPosition.y);
                int range = UnityEngine.Random.Range(-200, 200);
                spawnPoint.transform.localPosition = new Vector2(spawnPoint.transform.localPosition.x + range, spawnPoint.transform.localPosition.y);
                position = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y);
                return position;
            }
            position = spawnPoint.position;
            if (spawnPoint.localPosition.x == 0 || (spawnPoint.localPosition.x < _camWidth / 2 && spawnPoint.position.x > (_camWidth / 2) * -1))
            {
                int range = UnityEngine.Random.Range(-100, 101);
                position = new Vector2(spawnPoint.position.x + range, spawnPoint.position.y);
            }
            if (spawnPoint.localPosition.y == 100 || spawnPoint.localPosition.y == -100)
            {
                int range = UnityEngine.Random.Range(-100, 101);
                position = new Vector2(spawnPoint.position.x, spawnPoint.position.y + range);
            }

            return position;
        }

        public void StartWave(List<EnemySpawnData> enemyDatas) 
        {
            _currentWaveEnemies = enemyDatas;
        }

        private EnemyData GetEnemyData(EnemyType enemyType) 
        {
            return _gameplayData.GetEnemiesByType(enemyType);
        }

        private EnemySpawnData GetRandomEnemyFromWave()
        {
            //Maybe later change on special Random class with weight
            int randomIndex = Random.Range(0, _currentWaveEnemies.Count);
            return _currentWaveEnemies[randomIndex];
        }

        private Enemy Preload() => Instantiate(_enemyPrefab);

        public void GetAction(Enemy enemy)
        {
            EnemySpawnData enemySpawn = GetRandomEnemyFromWave();
            EnemyData enemyData = GetEnemyData(enemySpawn.enemyType);
            enemy = _enemyFactory.CreateEnemy(enemyData, enemy, enemyData.BuilderType);
            enemy.transform.position = GetSpawnPosition(enemySpawn.spawnType);
            enemy.gameObject.SetActive(true);
        }
        public void ReturnAction(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}

