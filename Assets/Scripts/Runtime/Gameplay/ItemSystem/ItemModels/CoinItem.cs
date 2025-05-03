using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CoinItem : ItemModel 
    {
        public int CoinAmount { get; }

        public CoinItem(int coinAmount, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            CoinAmount = coinAmount;
        }

        public override void ReleseItem(Vector3 position) 
        {
            EventBusHolder.EventBus.Raise(new CointItemReleaseEvent(CoinAmount));
        }
    }
}

