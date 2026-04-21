using System;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Bullet that survives multiple enemy hits before returning to pool.
    /// BulletData.BulletLife controls how many hits it can absorb.
    /// </summary>
    public class BulletWithHealth : BaseBullet
    {
        private float _bulletHealth;

        public override void Init(
            Vector3 startPosition,
            Vector3 targetPosition,
            Action<BaseBullet> returnToPoolCallback,
            BulletData bulletData,
            float damageMultiplier,
            float critChance,
            float critDamageMultiplier,
            float bulletSizeMultiplier)
        {
            base.Init(startPosition, targetPosition, returnToPoolCallback, bulletData,
                damageMultiplier, critChance, critDamageMultiplier, bulletSizeMultiplier);

            _bulletHealth = bulletData.BulletLife;
        }

        protected override void OnBulletHit()
        {
            _bulletHealth--;
            if (_bulletHealth <= 0)
                base.OnBulletHit();
        }
    }
}
