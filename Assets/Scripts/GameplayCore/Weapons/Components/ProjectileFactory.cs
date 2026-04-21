using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Manages a pool of bullets. Created per-weapon, not a MonoBehaviour.
    /// Pass plain float values for damage/crit/size — no modificator system required.
    /// </summary>
    public class ProjectileFactory : IProjectileFactory
    {
        private ObjectPool<BaseBullet> _pool;
        private readonly List<ITickable> _activeBullets = new();

        private Transform _bulletParent;
        private BulletData _bulletData;
        private Func<BaseBullet> _bulletCreator;

        private readonly float _damageMultiplier;
        private readonly float _critChance;
        private readonly float _critDamageMultiplier;
        private readonly float _bulletSizeMultiplier;

        public ProjectileFactory(
            BulletData bulletData,
            int preloadCount,
            Func<BaseBullet> bulletCreator,
            float damageMultiplier = 1f,
            float critChance = 0f,
            float critDamageMultiplier = 0f,
            float bulletSizeMultiplier = 1f)
        {
            _bulletData = bulletData;
            _bulletCreator = bulletCreator;
            _damageMultiplier = damageMultiplier;
            _critChance = critChance;
            _critDamageMultiplier = critDamageMultiplier;
            _bulletSizeMultiplier = bulletSizeMultiplier;

            CreateBulletParent();
            InitPool(preloadCount);
        }

        public void Tick()
        {
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
                _activeBullets[i].Tick();
        }

        public void CreateProjectile(Vector3 position, Vector3 direction)
        {
            BaseBullet bullet = _pool.Get();
            _activeBullets.Add(bullet);
            bullet.Init(position, direction, ReturnToPool, _bulletData,
                _damageMultiplier, _critChance, _critDamageMultiplier, _bulletSizeMultiplier);
        }

        /// <summary>Swap bullet data after Evolve (existing active bullets expire naturally).</summary>
        public void Evolve(BulletData newData, Func<BaseBullet> newCreator)
        {
            _bulletCreator = newCreator;
            _bulletData = newData;
        }

        private void CreateBulletParent()
        {
            _bulletParent = new GameObject("[BulletPool]").transform;
            _bulletParent.position = Vector3.zero;
        }

        private void InitPool(int preloadCount)
        {
            _pool = new ObjectPool<BaseBullet>(
                () =>
                {
                    BaseBullet b = _bulletCreator.Invoke();
                    b.transform.SetParent(_bulletParent);
                    return b;
                },
                bullet => bullet.Activate(),
                bullet =>
                {
                    _activeBullets.Remove(bullet);
                    bullet.Deactivate();
                },
                preloadCount);
        }

        private void ReturnToPool(BaseBullet bullet)
        {
            _pool.Return(bullet);
        }
    }
}
