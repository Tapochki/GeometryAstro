using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class BombItem : ItemModel
    {
        public int BombDamage { get; }

        public BombItem(int bombDamage, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            BombDamage = bombDamage;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new BombItemReleaseEvent(BombDamage));
        }
    }
}

