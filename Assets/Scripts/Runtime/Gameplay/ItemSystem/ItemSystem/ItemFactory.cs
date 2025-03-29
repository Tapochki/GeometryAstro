using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ItemFactory : IItemFactory
    {
        private readonly ItemConfig _itemConfig;

        public ItemFactory(ItemConfig itemConfig)
        {
            _itemConfig = itemConfig;
        }

        public ItemModel GetItemModel(ItemType type)
        {
            ItemData itemData = _itemConfig.GetItemDataByType(type);

            if (itemData == null)
            {
                Debug.LogError($"No data found for ItemType: {type}");
                return null;
            }

            int randomValue = UnityEngine.Random.Range(itemData.itemValueMin, itemData.itemValueMax + 1);

            return type switch
            {
                ItemType.Bomb => new BombItem(randomValue, itemData.sprite, type),
                ItemType.Chest => new ChestItem(randomValue, itemData.sprite, type),
                ItemType.Coin => new CoinItem(randomValue, itemData.sprite, type),
                ItemType.SmallXp or ItemType.MeduimXp or ItemType.BigXp => new ExpirienceItem(randomValue, itemData.sprite, type),
                ItemType.FrozenBomb => new FrozeBombItem(randomValue, itemData.sprite, type),
                ItemType.Magnet => new MagnetItem(itemData.sprite, type),
                ItemType.Medecine => new MedecineItem(randomValue, itemData.sprite, type),
                ItemType.RocketAmmo => new RocketAmmoItem(randomValue, itemData.sprite, type),
                _ => throw new ArgumentException($"Неизвестный тип предмета: {type}")
            };
        }
    }
}


