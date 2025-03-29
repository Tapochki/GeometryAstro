using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class FrozeBombItem : ItemModel 
    {
        public float FreezeTime { get; }

        public FrozeBombItem(int freezeTime, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            FreezeTime = freezeTime;
        }

        public override void ReleseItem() 
        {
            EventBusHolder.EventBus.Raise(new FrozeBombReleaseEvent(FreezeTime));
        }
    }
}

