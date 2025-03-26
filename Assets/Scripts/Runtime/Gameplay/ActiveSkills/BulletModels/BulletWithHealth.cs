using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class BulletWithHealth : BaseBullet
    {
        private float _bulletHealth = 2;

        public override void Init(Vector3 startPosition, Vector3 target, Action<BaseBullet> bulletBackToPoolEvent, BulletData bulletData, float damageModificatorMultiplier, float criticalChanceModificator, float criticalDamageMultiplier, float bulletSizeMultiplier)
        {
            base.Init(startPosition, target, bulletBackToPoolEvent, bulletData, damageModificatorMultiplier, criticalChanceModificator, criticalDamageMultiplier, bulletSizeMultiplier);
        }

        protected override void BulletHit()
        {
            _bulletHealth -= _damage;
            if (_bulletHealth <= 0)
            {
                BackToPool();
            }
        }
    }
}

