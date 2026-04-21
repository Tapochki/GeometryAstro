using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ExperienceItem : ItemModel 
    {
        public int ExpAmount { get; }

        public ExperienceItem(int expAmount, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            ExpAmount = expAmount;
        }

        public override void ReleseItem(Vector3 position) 
        {
            EventBusHolder.EventBus.Raise(new ExperienceItemReleaseEvent(ExpAmount));
        }
    }

}

