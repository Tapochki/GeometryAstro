using System;
using System.Collections.Generic;
using TandC.GeometryAstro.ConfigUtilities;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "TandC/Game/ItemConfig", order = 1)]
    public class ItemConfig : ScriptableObject, IJsonSerializable
    {
        [SerializeField] private List<ItemData> _itemData;

        public ItemData GetItemDataByType(ItemType itemType)
        {
            foreach (var item in _itemData)
            {
                if (item.type == itemType)
                {
                    return item;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class ItemData
    {
        public string Name;
        public int itemId;
        public int itemValueMin;
        public int itemValueMax;
        public Sprite sprite;
        public ItemType type;

        [TextArea(5, 10)]
        public string description;
    }
}