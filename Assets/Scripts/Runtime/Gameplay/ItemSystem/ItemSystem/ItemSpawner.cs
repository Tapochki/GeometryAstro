using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ItemSpawner : IItemSpawner
    {
        private const int ITEM_PRELOAD_COUNT = 200;

        private LoadObjectsService _loadObjectsService;

        private ItemView _itemViewPrefab;
        private Transform _itemParent;

        private ItemConfig _itemsConfig;
        private ChanceDropItemCofig _dropConfig;

        private Player _player;
        private IItemFactory _itemfactory;

        private ObjectPool<ItemView> _itemPool;

        private List<ItemView> _activeItems;

        [Inject]
        private void Construct(GameConfig gameConfig, Player player, LoadObjectsService loadObjectsService) 
        {
            _player = player;
            _itemsConfig = gameConfig.ItemConfig;
            _dropConfig = gameConfig.ChanceDropItemCofig;
            _loadObjectsService = loadObjectsService;
        }

        public void Init()
        {
            InitLists();
            LoadItemPrefab();
            CreateItemParent();
            InitializeItemFactory();
            InitializePool();
        }

        private void InitLists() 
        {
            _activeItems = new List<ItemView>();
        }

        private void CreateItemParent()
        {
            _itemParent = new GameObject("ITEMS").transform;
        }

        private void LoadItemPrefab()
        {
            if (_itemViewPrefab == null)
            {
                _itemViewPrefab = _loadObjectsService.GetObjectByPath<ItemView>("Prefabs/Gameplay/Items/ItemPrefab");
                if (_itemViewPrefab == null)
                {
                    Debug.LogError("Failed to load Item prefab from the specified path.");
                }
            }
        }

        private void InitializeItemFactory() 
        {
            _itemfactory = new ItemFactory(_itemsConfig);
        }

        private void InitializePool()
        {
            _itemPool = new ObjectPool<ItemView>(Preload, GetItem, BackItemToPool, ITEM_PRELOAD_COUNT);
        }

        public void DropRandomItem(DropItemRareType type, Vector2 spawnPosition) 
        {
            ItemType itemType = _dropConfig.GetDropDataByType(type).GetRandomItemType();
            DropItem(itemType, spawnPosition);
        }

        private void DropItem(ItemType itemType, Vector2 spawnPosition) 
        {           
            ItemView itemView = _itemPool.Get();
            ItemModel itemData = _itemfactory.GetItemModel(itemType);
            itemView.Init(ReturnToPool, _player.transform, itemData.ItemSprite, _itemfactory.GetItemModel(itemType));
            itemView.transform.position = spawnPosition;
            itemView.gameObject.SetActive(true);
            _activeItems.Add(itemView);
        }

        private void ReturnToPool(ItemView itemView) 
        {
            _itemPool.Return(itemView);
        }

        private ItemView Preload() => MonoBehaviour.Instantiate(_itemViewPrefab, _itemParent);

        private void GetItem(ItemView item) { }

        private void BackItemToPool(ItemView item) 
        {
            item.gameObject.SetActive(false);
            _activeItems.Remove(item);
        }
    }
}


