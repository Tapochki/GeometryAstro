using System.Collections.Generic;
using TandC.Settings;
using UnityEngine;
using Zenject;

namespace TandC.Gameplay 
{
    public class ItemFactory : MonoBehaviour, IItemFactory
    {
        private Dictionary<ItemType, ItemModel> _itemModels;

        private LevelModel _levelModel;

        [Inject]
        private void Construct(LevelModel levelModel) 
        {
            _levelModel = levelModel;
        }

        private void Start()
        {
            _itemModels = new Dictionary<ItemType, ItemModel>();
            InitializeExpirienceItemModel();
            InitializeMedecineItemModel();
        }

        private void InitializeExpirienceItemModel()
        {
            _itemModels.Add(ItemType.SmallXp, new ExpirienceItemModel(_levelModel));
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


