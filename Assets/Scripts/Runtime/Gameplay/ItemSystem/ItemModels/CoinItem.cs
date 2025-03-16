using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CoinItem : ItemModel 
    {
        public int CoinAmount { get; }

        public CoinItem(int coinAmount, Sprite itemSprite) : base(itemSprite)
        {
            CoinAmount = coinAmount;
        }

        public override void ReleseItem() 
        {
            EventBusHolder.EventBus.Raise(new CointItemReleaseEvent(CoinAmount));
        }
    }
}

