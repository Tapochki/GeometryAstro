using UnityEngine;

namespace TandC.Gameplay 
{
    public abstract class ItemModel
    {
        public abstract void ReleseItem();
    }

    public class ExpirienceItemModel : ItemModel 
    {
        private LevelModel _levelModel;

        public ExpirienceItemModel(LevelModel levelModel) 
        {
            _levelModel = levelModel;
        }

        public override void ReleseItem() 
        {
            _levelModel.AddExpirience(50);//TODO change to item
        }
    }

    public class MedecineItemModel : ItemModel
    {
        public override void ReleseItem()
        {
            Debug.LogError("MedecineItemModel");
        }
    }
}

