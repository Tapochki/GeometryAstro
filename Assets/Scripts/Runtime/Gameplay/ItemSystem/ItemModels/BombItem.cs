using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class BombItem : ItemModel
    {
        private float _explosionArea = 20f;

        public int BombDamage { get; }
        private ExplosionDamage _explosionDamage;

        public BombItem(int bombDamage, Sprite itemSprite, ItemType type) : base(itemSprite, type)
        {
            _explosionDamage = new ExplosionDamage();
            BombDamage = bombDamage;
        }

        public override void ReleseItem(Vector3 position)
        {
            EventBusHolder.EventBus.Raise(new CreateExplosionEffect(position, _explosionArea));
            _explosionDamage.ApplyExplosionDamage(position, _explosionArea, BombDamage);
        }
    }
}

