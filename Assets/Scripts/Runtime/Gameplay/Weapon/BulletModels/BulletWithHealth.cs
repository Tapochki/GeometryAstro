using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class BulletWithHealth : BaseBullet
    {
        private float _bulletHealth = 2;

        public override void Init(Vector2 startPosition, Vector2 target, Action<BaseBullet> bulletBackToPoolEvent, BulletData bulletData, float damage)
        {
            base.Init(startPosition, target, bulletBackToPoolEvent, bulletData, damage);
        }

        protected override void BulletHit()
        {
            _bulletHealth -= _damage;
            if (_bulletHealth <= 0)
            {
                Dispose();
            }
        }
    }
}

