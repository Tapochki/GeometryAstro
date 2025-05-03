using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MagnetItem : ItemModel
    {
        public MagnetItem(Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            
        }

        public override void ReleseItem(Vector3 position)
        {
            EventBusHolder.EventBus.Raise(new MagnetItemReleaseEvent());
        }
    }
}

