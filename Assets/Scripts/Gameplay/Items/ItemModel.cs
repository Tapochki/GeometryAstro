using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TandC.Gameplay 
{
    public abstract class ItemModel
    {
        public abstract void ReleseItem();
    }

    public class ExpirienceItemModel : ItemModel 
    {
        public override void ReleseItem() 
        {
            Debug.LogError("ExpirienceItemModel");
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

