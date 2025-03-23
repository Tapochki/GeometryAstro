using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class FrozeBombItem : ItemModel 
    {
        public float FreezeTime { get; }

        public FrozeBombItem(int freezeTime, Sprite itemSprite) : base(itemSprite)
        {
            FreezeTime = freezeTime;
        }

        public override void ReleseItem() 
        {
            EventBusHolder.EventBus.Raise(new FrozeBombReleaseEvent(FreezeTime));
        }
    }
}

