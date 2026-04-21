using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MedicineItem : ItemModel
    {
        public int HealAmount { get; }

        public MedicineItem(int healAmount, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            HealAmount = healAmount;
        }

        public override void ReleseItem(Vector3 position)
        {
            EventBusHolder.EventBus.Raise(new PlayerHealReleaseEvent(HealAmount));
        }
    }
}

