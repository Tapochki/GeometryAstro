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

        private TickService _tickService;

        private IItemSpawner _itemSpawner;

        private LevelConfig _levelConfig;

        private IReadableModificator _curseStrenghtModificator;
        private IReadableModificator _curseSpeedModificator;

        private Enemy _enemyPrefab;
        private GameObject _enemyParent;

        private LoadObjectsService _loadObjectsService;
        private Player _player;
        private EnemyConfig _enemiesConfig;
        private IEnemyFactory _enemyFactory;
        private IEnemySpawnPositionService _enemySpawnPositionService;
        private ObjectPool<Enemy> _enemyPool;

        private EnemyFreezeProcessor _enemyFreezeProcessor;

        private List<IEnemy> _activeEnemyList;
        private List<EnemySpawnData> _currentWaveEnemies;

        private ScoreContainer _scoreContainer;

        public int ActiveEnemyCount => _activeEnemyList.Count;

        private ModificatorContainer _modificatorContainer;

        [Inject]
        private void Construct(
            LoadObjectsService loadObjectsService,
            IEnemySpawnPositionService enemySpawnPositionService,
            IItemSpawner itemSpawner,
            GameConfig gameConfig,
            ScoreContainer scoreContainer,
            Player player,
            TickService tickService,
            ModificatorContainer modificatorContainer)
        {
            _loadObjectsService = loadObjectsService;
            _player = player;
            _scoreContainer = scoreContainer;
            _enemySpawnPositionService = enemySpawnPositionService;
            _enemiesConfig = gameConfig.EnemyConfig;
            _levelConfig = gameConfig.LevelsConfig;
            _itemSpawner = itemSpawner;
            _tickService = tickService;
            _modificatorContainer = modificatorContainer;
        }

        public void Init()
        {
            CreateEnemyParent();
            LoadEnemyPrefab();
            InitLists();
            InitFreezeProcessor();
            InitializePool();
            InitModificators();

            _tickService.RegisterUpdate(Tick);
        }

        private void InitModificators() 
        {
            _curseSpeedModificator = _modificatorContainer.GetModificator(ModificatorType.CurseSpeed);
            _curseStrenghtModificator = _modificatorContainer.GetModificator(ModificatorType.CurseStrength);
        }

        private void InitFreezeProcessor()
        {
            _enemyFreezeProcessor = new EnemyFreezeProcessor(FreezeAllEnemy);
        }

        private void CreateEnemyParent() 
        {
            _enemyParent = new GameObject("[ENEMY]");
        }

        private void LoadEnemyPrefab() 
        {
            _enemyPrefab = _loadObjectsService.GetObjectByPath<Enemy>("Prefabs/Gameplay/Enemies/BasicEnemy");
            if (_enemyPrefab == null)
            {
                Debug.LogError("Enemy prefab not found at path: Resources/Prefabs/Gameplay/Enemies/BasicEnemy");
            }
        }

        private void InitLists() 
        {
            _activeEnemyList = new List<IEnemy>();
            _enemyFactory = new EnemyFactory();
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
                SetupEnemy(enemy, enemyData, spawnPoint.position);
                _activeEnemyList.Add(enemy);
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
                onDeathEvent: (enemy, isKilled) => HandleEnemyDeath(enemy, isKilled),
                target: _player.transform,
                moveDirection: direction,
                builderType: data.BuilderType,
                CalculateHealthModificator(),
                CalculateSpeedModificator(),
                CalculateDamageModificator()
            );
            enemy.transform.position = spawnPosition;
        }

        private float CalculateHealthModificator() 
        {
            return _levelConfig.GetHealthMultiplier() + _curseStrenghtModificator.Value - 1;
        }

        private float CalculateDamageModificator() 
        {
            return _levelConfig.GetDamageMultiplier() + _curseStrenghtModificator.Value - 1;
        }

        private float CalculateSpeedModificator() 
        {
            return _levelConfig.GetSpeedMultiplier() + _curseSpeedModificator.Value - 1;
        }

        private float CalculateScoreModificator()
        {
            return _levelConfig.GetScoreMultiplier() + _curseSpeedModificator.Value + _curseStrenghtModificator.Value - 1;
        }

        private void HandleEnemyDeath(Enemy enemy, bool isKilled)
        {
            if(isKilled)
                ProccesDropItemFromEnemy(enemy);

            _activeEnemyList.Remove(enemy);
            _enemyPool.Return(enemy);
        }

        private void Tick() 
        {
            for (int i = _activeEnemyList.Count - 1; i >= 0; i--)
            {
                IEnemy enemy = _activeEnemyList[i];
                enemy.Tick();
            }
        }

        private void ProccesDropItemFromEnemy(Enemy enemy)
        {
            _scoreContainer.AddScore(enemy.EnemyData.Score, CalculateScoreModificator());
            _itemSpawner.DropRandomItem(enemy.EnemyData.droperType, enemy.transform.position);
        }

        private void FreezeAllEnemy(float timer) 
        {
            for (int i = _activeEnemyList.Count - 1; i >= 0; i--)
            {
                IEnemy enemy = _activeEnemyList[i];
                enemy.Freeze();
            }
        }

        public void SpawnBoss(EnemySpawnData bossData, Action onBossDefeated)
        {
            //Boss spawn
        }
    }
}