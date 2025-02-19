using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;
namespace TandC.GeometryAstro.Gameplay 
{
    public class ItemFactory : MonoBehaviour, IItemFactory
    {
        private Dictionary<ItemType, ItemModel> _itemModels;

        private LevelModel _levelModel;

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


