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
                ItemType.Bomb => new BombItem(randomValue, itemData.sprite),
                ItemType.Chest => new ChestItem(randomValue, itemData.sprite),
                ItemType.Coin => new CoinItem(randomValue, itemData.sprite),
                ItemType.SmallXp or ItemType.MeduimXp or ItemType.BigXp => new ExpirienceItem(randomValue, itemData.sprite),
                ItemType.FrozenBomb => new FrozeBombItem(randomValue, itemData.sprite),
                ItemType.Magnet => new MagnetItem(itemData.sprite),
                ItemType.Medecine => new MedecineItem(randomValue, itemData.sprite),
                ItemType.RocketAmmo => new RocketAmmoItem(randomValue, itemData.sprite),
                _ => throw new ArgumentException($"Неизвестный тип предмета: {type}")
            };
        }
    }
}


