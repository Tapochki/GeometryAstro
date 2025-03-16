using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketAmmoItem : ItemModel
    {
        public int AmmoCount { get; }

        public RocketAmmoItem(int ammoCount, Sprite itemSprite) : base(itemSprite)
        {
            AmmoCount = ammoCount;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new RocketAmmoItemReleaseEvent(AmmoCount));
        }
    }
}

