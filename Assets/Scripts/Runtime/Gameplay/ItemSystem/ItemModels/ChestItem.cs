using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ChestItem : ItemModel
    {
        public int ChestItemCount { get; }

        public ChestItem(int chestItemCount, Sprite itemSprite) : base(itemSprite)
        {
            ChestItemCount = chestItemCount;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new ChestItemReleaseEvent());
        }
    }
}

