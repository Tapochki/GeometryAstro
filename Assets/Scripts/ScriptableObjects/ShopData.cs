using ChebDoorStudio.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChebDoorStudio.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShopData", menuName = "ChebDoorStudio/ShopData", order = 3)]
    public class ShopData : ScriptableObject
    {
        public List<ShopItemData> items;

        public ShopItemData GetItemByType(ShopItemType type)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].type == type)
                {
                    return items[i];
                }
            }
            return null;
        }
    }

    [Serializable]
    public class ShopItemData
    {
        public ShopItemType type;
        public Sprite itemPreview;
        public int price;
    }
}