using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ChestItem : ItemModel
    {
        public int ChestItemCount { get; }

        public ChestItem(int chestItemCount, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            ChestItemCount = chestItemCount;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new ChestItemReleaseEvent());
        }
    }
}

