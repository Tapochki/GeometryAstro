using System.Collections.Generic;
using TandC.Settings;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class ItemFactory : MonoBehaviour, IItemFactory
    {
        private Dictionary<ItemType, ItemModel> _itemModels;

        private void Start()
        {
            _itemModels = new Dictionary<ItemType, ItemModel>();
            InitializeExpirienceItemModel();
            InitializeMedecineItemModel();
        }

        private void InitializeExpirienceItemModel()
        {
            _itemModels.Add(ItemType.SmallXp, new ExpirienceItemModel());
        }

        private void InitializeMedecineItemModel()
        {
            _itemModels.Add(ItemType.Medecine, new MedecineItemModel());
        }

        public ItemModel GetItemModel(ItemType type) 
        {
            return _itemModels[type];
        }
    }
}


