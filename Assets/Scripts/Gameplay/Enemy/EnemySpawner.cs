using System.Collections;
using System.Collections.Generic;
using TandC.Data;
using TandC.Settings;
using TandC.Utilities;
using UnityEngine;

namespace TandC.Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        private const int ENEMY_PRELOAD_COUNT = 200;
        [SerializeField] private GameplayData _gameplayData;
        [SerializeField] EnemyFactory _enemyFactory;
        [SerializeField] Camera _camera;
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private Player _player;

        private float _camHeight;
        private float _camWidth;

        private ObjectPool<Enemy> _enemyPool;
        private List<EnemyData> _currentWaveEnemies;

        public void Start()
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            _camHeight = _camera.orthographicSize;
            _camWidth = screenAspect * _camHeight;
            _enemyPool = new ObjectPool<Enemy>(Preload, GetAction, ReturnAction, ENEMY_PRELOAD_COUNT);
            _currentWaveEnemies = new List<EnemyData>();
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

        private void SpecialSpawn(int spawnPointCountStart, int spawnPointCountEnd, Enemy enemy)
        {
            for (int i = spawnPointCountStart; i <= spawnPointCountEnd; i++)
            {
                SpawnEnemy(enemy, _spawnPositions[i].position);
            }
        }

        //public void StartSpawnEnemy()
        //{
        //    if (_allEnemies.Count >= 200)
        //    {
        //        return;
        //    }
        //    int enemyInPhaseId = 0;
        //    if (_currentPhase.IsRandomEnemySpawn)
        //    {
        //        enemyInPhaseId = UnityEngine.Random.Range(0, _enemysInPhase.Count);
        //    }

        //    EnemySpawnData enemySpawnData = _enemysInPhase[enemyInPhaseId];
        //    _enemysInPhase.Remove(enemySpawnData);
        //    switch (enemySpawnData.spawnType)
        //    {
        //        case SpawnType.Circle:
        //            SpecialSpawn(0, _spawnsType.Count - 1, enemySpawnData.enemyType);
        //            break;
        //        case SpawnType.UpperPosition:
        //            SpecialSpawn(0, 4, enemySpawnData.enemyType);
        //            break;
        //        case SpawnType.LeftPosition:
        //            SpecialSpawn(4, 6, enemySpawnData.enemyType);
        //            break;
        //        case SpawnType.DownPosition:
        //            SpecialSpawn(6, 10, enemySpawnData.enemyType);
        //            break;
        //        case SpawnType.RightPosition:
        //            _enemies.Add(SpawnEnemy(_gameplayData.GetEnemiesByType(enemySpawnData.enemyType), SpawnType.EnemySpawnPosition_0, false));
        //            SpecialSpawn(10, _spawnsType.Count - 1, enemySpawnData.enemyType);
        //            break;
        //        default:
        //            _enemies.Add(SpawnEnemy(_gameplayData.GetEnemiesByType(enemySpawnData.enemyType), enemySpawnData.spawnType, false));
        //            break;
        //    }
        //}

        public void StartWave(List<EnemyData> enemyDatas) 
        {
            _enemyPool = new ObjectPool<Enemy>(Preload, GetAction, ReturnAction, ENEMY_PRELOAD_COUNT);
            _currentWaveEnemies = enemyDatas;
        }

        private void SpawnEnemy(Enemy enemy, Vector2 position)
        {
            
        }

        private Enemy Preload()
        {
            Enemy enemy = _enemyFactory.CreateEnemy(data, data.type);
            return enemy;
        }
        public void GetAction(Enemy enemy) 
        {
            enemy.transform.position = position;
            SpawnEnemy(enemy, position);
            enemy.gameObject.SetActive(true);
        }
        public void ReturnAction(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}

