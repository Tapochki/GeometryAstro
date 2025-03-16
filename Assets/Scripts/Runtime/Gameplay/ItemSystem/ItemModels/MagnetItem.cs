using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MagnetItem : ItemModel
    {
        public MagnetItem(Sprite itemSprite) : base(itemSprite)
        {
            
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new MagnetItemReleaseEvent());
        }
    }
}

