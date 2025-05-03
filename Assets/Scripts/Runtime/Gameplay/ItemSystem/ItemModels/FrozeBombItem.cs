using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class FrozeBombItem : ItemModel 
    {
        private float _explosionArea = 300f;

        public float FreezeTime { get; }

        public FrozeBombItem(int freezeTime, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            FreezeTime = freezeTime;
        }

        public override void ReleseItem(Vector3 position) 
        {
            EventBusHolder.EventBus.Raise(new FrozeBombReleaseEvent(FreezeTime));
            EventBusHolder.EventBus.Raise(new CreateFreezeEffect(position, _explosionArea));
        }
    }
}

