using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ExpirienceItem : ItemModel 
    {
        public int ExpAmount { get; }

        public ExpirienceItem(int expAmount, Sprite itemSprite) : base(itemSprite)
        {
            ExpAmount = expAmount;
        }

        public override void ReleseItem() 
        {
            EventBusHolder.EventBus.Raise(new ExpirienceItemReleaseEvent(ExpAmount));
        }
    }

}

