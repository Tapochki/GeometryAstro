using System;
using System.Collections.Generic;
using TandC.Data;
using TandC.Settings;
using TandC.Utilities;
using UnityEngine;
using Zenject;

namespace TandC.Gameplay 
{
    public class ItemSpawner : MonoBehaviour
    {
        private const int ITEM_PRELOAD_COUNT = 200;

        [SerializeField]
        private GameplayData _gameplayData;
        [SerializeField]
        private ItemView _itemViewPrefab;
        [SerializeField]
        private Transform _itemParent;
        [SerializeField]
        private Player _player;

        private ItemFactory _itemfactory;
        private Dictionary<DropItemRareType, RandomDroper<ItemData>> _itemsRandomDropers;
        private RandomDropItemFactory _randomDroperFactory;


        private ObjectPool<ItemView> _itemPool;

        [Inject]
        private void Construct(ItemFactory itemFactory) 
        {
            _itemfactory = itemFactory;
        }

        private void Start()
        {
            _randomDroperFactory = new RandomDropItemFactory(_gameplayData);
            InitializeDrops();
            InitializePool();
        }
        private void InitializePool()
        {
            _itemPool = new ObjectPool<ItemView>(Preload, GetItem, BackItemToPool, ITEM_PRELOAD_COUNT);
        }
        private void InitializeDrops()
        {
            _itemsRandomDropers = new Dictionary<DropItemRareType, RandomDroper<ItemData>>()
            {
                {DropItemRareType.DefaultDrop, _randomDroperFactory.InitializeRandomDroper(DropItemRareType.DefaultDrop)},
                {DropItemRareType.RareDrop, _randomDroperFactory.InitializeRandomDroper(DropItemRareType.RareDrop)},
                {DropItemRareType.BossDrop, _randomDroperFactory.InitializeRandomDroper(DropItemRareType.BossDrop)}
            };
        }

        public void DropItem(DropItemRareType type, Vector2 spawnPosition) 
        {
            ItemData itemData = _itemsRandomDropers[type].GetDrop();
            if(itemData == null) 
            {
                return;
            }
            Debug.LogError(itemData.type);
            ItemView itemView = _itemPool.Get();
            itemView.Init(BackItemToPool, _player.transform, itemData.sprite, _itemfactory.GetItemModel(itemData.type));
            itemView.transform.position = spawnPosition;
            itemView.gameObject.SetActive(true);
        }

        private ItemView Preload() => Instantiate(_itemViewPrefab, _itemParent);

        private void GetItem(ItemView item) { }

        private void BackItemToPool(ItemView item) { item.gameObject.SetActive(false); }

    }
    public class RandomDropItemFactory
    {
        private GameplayData _gameplayData;

        public RandomDropItemFactory(GameplayData gameplayData) 
        {
            _gameplayData = gameplayData;
        }

        public RandomDroper<ItemData> InitializeRandomDroper(DropItemRareType dropType)
        {
            switch (dropType)
            {
                case DropItemRareType.DefaultDrop:
                    return CreateRandomDroper(new List<RandomDropItem<ItemData>>(){
                    { CreateEmptyDropItem(10) },
                    { CreateRandomDropItem(ItemType.SmallXp, 100) },
                    { CreateRandomDropItem(ItemType.Medecine, 10) },
                });
                case DropItemRareType.RareDrop:
                    return CreateRandomDroper(new List<RandomDropItem<ItemData>>()
                {
                    { CreateEmptyDropItem(100) },
                    { CreateRandomDropItem(ItemType.MeduimXp, 10) },
                    { CreateRandomDropItem(ItemType.Medecine, 10) },
                    { CreateRandomDropItem(ItemType.Bomb, 10) },
                });
                case DropItemRareType.BossDrop:
                    return CreateRandomDroper(new List<RandomDropItem<ItemData>>()
                {
                    { CreateRandomDropItem(ItemType.Chest, 100) },
                    { CreateRandomDropItem(ItemType.BigXp, 100) },
                });
            }
            throw new InvalidOperationException();
        }

        private RandomDroper<ItemData> CreateRandomDroper(List<RandomDropItem<ItemData>> itemList)
        {
            return new RandomDroper<ItemData>(itemList);
        }

        private RandomDropItem<ItemData> CreateRandomDropItem(ItemType type, float weight)
        {
            return new RandomDropItem<ItemData>(_gameplayData.GetItemDataByType(type), weight);
        }

        private RandomDropItem<ItemData> CreateEmptyDropItem(float weight)
        {
            return new RandomDropItem<ItemData>(null, weight);
        }
    }
}


