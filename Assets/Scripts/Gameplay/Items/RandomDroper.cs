using System.Collections.Generic;
using TandC.Utilities;

namespace TandC.Gameplay 
{
    public class RandomDroper<T>
    {
        private List<RandomDropItem<T>> _dropItemsList;

        private float _totalWeight;
        public RandomDroper(List<RandomDropItem<T>> dropList)
        {
            _dropItemsList = dropList;

            _totalWeight = GetTotalWeight();

        }

        private float GetTotalWeight()
        {
            float totalWeight = 0f;
            foreach (var item in _dropItemsList)
            {
                totalWeight += item.Weight;
            }
            return totalWeight;
        }

        public T GetDrop()
        {
            if (_dropItemsList.Count == 1)
            {
                return _dropItemsList[0].Item;
            }

            float roll = UnityEngine.Random.Range(0f, _totalWeight);
            foreach (var item in _dropItemsList)
            {
                if (item.Weight >= roll)
                {
                    return item.Item;
                }

                roll -= item.Weight;
            }

            throw new System.Exception("Failed to generate drop!");
        }
    }

    public class RandomDropItem<T>
    {
        public float Weight { get; private set; }
        public T Item { get; private set; }

        public RandomDropItem(T item, float weight) 
        {
            Item = item;
            Weight = weight >= 0f ? weight : 0f;
        }
    }
}

