using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MedecineItem : ItemModel
    {
        public int HealAmount { get; }

        public MedecineItem(int healAmount, Sprite itemSprite) : base(itemSprite)
        {
            HealAmount = healAmount;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new MedecineItemReleaseEvent(HealAmount));
        }
    }
}

