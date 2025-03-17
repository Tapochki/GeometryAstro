using System;
using System.Collections.Generic;
using TandC.GeometryAstro.ConfigUtilities;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "ChanceDropItemCofig", menuName = "TandC/Game/ChanceDropItemCofig", order = 1)]
    public class ChanceDropItemCofig : ScriptableObject, IJsonSerializable
    {
        [SerializeField] private List<DropData> _dropData;

        public DropData GetDropDataByType(DropItemRareType dropType)
        {
            foreach (var drop in _dropData)
            {
                if (drop.dropType == dropType)
                {
                    return drop;
                }
            }

            return null;
        }
    }
    [Serializable]
    public class DropData
    {
        public DropItemRareType dropType;
        public List<DropItemChance> itemsWithChance;

        public ItemType GetRandomItemType()
        {
            float totalWeight = 0f;
            foreach (var drop in itemsWithChance)
            {
                totalWeight += drop.weight;
            }

            float randomValue = UnityEngine.Random.Range(0f, totalWeight);
            float cumulativeWeight = 0f;

            foreach (var drop in itemsWithChance)
            {
                cumulativeWeight += drop.weight;
                if (randomValue <= cumulativeWeight)
                {
                    return drop.itemType;
                }
            }

            return default;
        }
    }

    [Serializable]
    public struct DropItemChance
    {
        public ItemType itemType;
        public float weight;
    }
}