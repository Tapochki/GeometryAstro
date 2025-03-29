using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketAmmoItem : ItemModel
    {
        public int AmmoCount { get; }

        public RocketAmmoItem(int ammoCount, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            AmmoCount = ammoCount;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new RocketAmmoItemReleaseEvent(AmmoCount));
        }
    }
}

