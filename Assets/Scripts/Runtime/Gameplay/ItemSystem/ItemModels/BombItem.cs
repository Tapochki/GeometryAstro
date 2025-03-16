using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class BombItem : ItemModel
    {
        public int BombDamage { get; }

        public BombItem(int bombDamage, Sprite itemSprite) : base(itemSprite)
        {
            BombDamage = bombDamage;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new BombItemReleaseEvent(BombDamage));
        }
    }
}

