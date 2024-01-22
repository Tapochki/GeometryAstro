using ChebDoorStudio.Gameplay.Items.Base;
using ChebDoorStudio.Gameplay.Player;
using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.ScriptableObjects;
using ChebDoorStudio.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.SceneSystems
{
    public class ItemSpawnSystem : MonoBehaviour
    {
        public event Action<ItemTypes> OnItemPickupEvent;

        private float _itemSpawnTime;
        private float _currentItemSpawnTime;

        private int _spawnIndex;

        private InitialGameData _initialGameData;
        private GameStateSystem _gameStateSystem;
        private PlayerComponent _playerComponent;
        private LoadObjectsSystem _loadObjectsSystem;

        private bool _startSpawning;
        private bool _isReadyToSpawnItem;
        private bool _needToSpawnItemAsap;

        private GameObject _particlePickupPrefab;

        private Transform _itemParent;

        private List<ItemBase> _items;

        private List<Transform> _itemSpawnPoints;

        [Inject]
        public void Construct(InitialGameData initialGameData, GameStateSystem gameStateSystem,
                                PlayerComponent playerComponent, LoadObjectsSystem loadObjectsSystem)
        {
            _initialGameData = initialGameData;
            _gameStateSystem = gameStateSystem;
            _playerComponent = playerComponent;
            _loadObjectsSystem = loadObjectsSystem;

            _itemSpawnTime = _initialGameData.itemData.timeToSpawnItem;

            _itemParent = GameObject.Find("[GAMEPLAY]/[ITEMS]").transform;
            _particlePickupPrefab = _loadObjectsSystem.GetObjectByPath<GameObject>("Prefabs/Particle_Pickup");

            _items = new List<ItemBase>();

            _itemSpawnPoints = new List<Transform>();

            FillItemSpawnPoints();

            _gameStateSystem.OnGameplayStopedEvent += OnGameplayStopedEventHandler;
            _gameStateSystem.OnGameplayStartedEvent += OnGameplayStartedEventHandler;
            _playerComponent.OnPlayerDeathEvent += OnPlayerDeathEventHandler;
        }

        private void ResetSpawnTime() => _currentItemSpawnTime = _itemSpawnTime;

        private ItemData GetItemData() => _initialGameData.itemData;

        private ItemBase GetRandomItem() => GetItemData().items[UnityEngine.Random.Range(0, GetItemData().items.Count)];

        private void Update()
        {
            if (_startSpawning)
            {
                _currentItemSpawnTime -= Time.deltaTime;

                if (_currentItemSpawnTime <= 0)
                {
                    _isReadyToSpawnItem = true;

                    _startSpawning = false;
                }
            }

            if (_needToSpawnItemAsap)
            {
                if (_isReadyToSpawnItem)
                {
                    SpawnItem();

                    ResetSpawnTime();

                    _needToSpawnItemAsap = false;
                    _isReadyToSpawnItem = false;

                    _startSpawning = true;
                }
            }
        }

        private void FillItemSpawnPoints()
        {
            for (int i = 0; i < _itemParent.childCount; i++)
            {
                _itemSpawnPoints.Add(_itemParent.GetChild(i));
            }
        }

        private void SpawnItem()
        {
            ItemBase itemToSpawn = MonoBehaviour.Instantiate(GetRandomItem(), _itemSpawnPoints[_spawnIndex]);
            itemToSpawn.Initialize();

            itemToSpawn.OnItemPickupEvent += OnItemPickupEventHandler;

            _items.Add(itemToSpawn);

            _spawnIndex++;

            if (_spawnIndex >= _itemSpawnPoints.Count)
            {
                _spawnIndex = 0;
            }

            ResetSpawnTime();
        }

        public void Dispose()
        {
            _gameStateSystem.OnGameplayStopedEvent -= OnGameplayStopedEventHandler;
            _gameStateSystem.OnGameplayStartedEvent -= OnGameplayStartedEventHandler;
            _playerComponent.OnPlayerDeathEvent -= OnPlayerDeathEventHandler;
        }

        private void OnItemPickupEventHandler(ItemBase item)
        {
            var particle = Instantiate(_particlePickupPrefab);
            particle.transform.position = item.transform.position;

            particle = null;

            _items.Remove(item);
            Destroy(item.gameObject);

            if (_isReadyToSpawnItem)
            {
                SpawnItem();

                _startSpawning = true;
                _isReadyToSpawnItem = false;
            }
            else
            {
                _needToSpawnItemAsap = true;
                _startSpawning = true;
                _isReadyToSpawnItem = false;
            }

            OnItemPickupEvent?.Invoke(item.ItemType);
        }

        private void OnPlayerDeathEventHandler()
        {
            _startSpawning = false;
            _needToSpawnItemAsap = false;
            _isReadyToSpawnItem = false;
        }

        private void OnGameplayStopedEventHandler()
        {
            _startSpawning = false;
            _needToSpawnItemAsap = false;
            _isReadyToSpawnItem = false;

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Dispose();
                Destroy(_items[i].gameObject);
            }

            _items.Clear();
        }

        private void OnGameplayStartedEventHandler()
        {
            _startSpawning = true;

            _needToSpawnItemAsap = true;
            _isReadyToSpawnItem = false;

            _spawnIndex = UnityEngine.Random.Range(0, _itemSpawnPoints.Count);

            ResetSpawnTime();
        }
    }
}