using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ExpirienceItem : ItemModel 
    {
        public int ExpAmount { get; }

        public ExpirienceItem(int expAmount, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            ExpAmount = expAmount;
        }

        public override void ReleseItem() 
        {
            EventBusHolder.EventBus.Raise(new ExpirienceItemReleaseEvent(ExpAmount));
        }
    }

}

