using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MedecineItem : ItemModel
    {
        public int HealAmount { get; }

        public MedecineItem(int healAmount, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            HealAmount = healAmount;
        }

        public override void ReleseItem()
        {
            EventBusHolder.EventBus.Raise(new PlayerHealReleaseEvent(HealAmount));
        }
    }
}

