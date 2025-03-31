using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ProjectileFactory : IProjectileFactory
    {
        private ObjectPool<BaseBullet> _pool;
        private List<ITickable> _activeBullet;
        private Transform _projectileParent;
        private BulletData _bulletData;
        private readonly int _startBulletPreloadCount;

        private Func<BaseBullet> _bulletCreator;

        private IReadableModificator _damageModificator;
        private IReadableModificator _criticalChanceModificator;
        private IReadableModificator _criticalDamageMultiplier;
        private IReadableModificator _bulletSize;

        public ProjectileFactory(BulletData bulletData, 
            int startBulletPreloadCount,
            Func<BaseBullet> bulletCreator,
            IReadableModificator damageModificator, 
            IReadableModificator criticalChangeModificator, 
            IReadableModificator criticalDamageMultiplier, 
            IReadableModificator bulletSize)

        {
            _activeBullet = new List<ITickable>();
            _damageModificator = damageModificator;
            _bulletData = bulletData;
            _startBulletPreloadCount = startBulletPreloadCount;
            _bulletCreator = bulletCreator;
            CreateBulletParent();
            InitializePool();
            _criticalChanceModificator = criticalChangeModificator;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _bulletSize = bulletSize;
        }

        public void Tick() 
        {
            for (int i = _activeBullet.Count - 1; i >= 0; i--)
            {
                ITickable bullet = _activeBullet[i];
                bullet.Tick();
            }
        }

        private void CreateBulletParent()
        {
            _projectileParent = new GameObject($"[{_bulletData.type}]").transform;
            _projectileParent.position = Vector3.zero;
        }

        private void InitializePool()
        {
            _pool = new ObjectPool<BaseBullet>(
                preloadFunc: () => CreateBullet(),
                getAction: bullet => bullet.Activate(),
                returnAction: bullet => DeactivateBullet(bullet),
                _startBulletPreloadCount);
        }

        private BaseBullet CreateBullet()
        {
            Debug.LogError("CreateBullet");
            var bulletObj = _bulletCreator.Invoke();
            bulletObj.transform.SetParent(_projectileParent);
            return bulletObj.GetComponent<BaseBullet>();
        }

        public void Evolve(BulletData newEvolveData, Func<BaseBullet> newBulletCreator) 
        {
            _bulletCreator = newBulletCreator;

            _bulletData = newEvolveData;

            ClearOldBullets();

            InitializePool();
        }

        private void ClearOldBullets()
        {
            var oldBulletsInPool = _pool.GetAllItemInPool();
            var activeOldBulletsBullets = _pool.GetAllActiveItem();

            DestroyOldBullets(oldBulletsInPool, activeOldBulletsBullets);

            _pool.Dispose();
        }

        private void ReturnBulletToPool(BaseBullet bullet) 
        {
            if(bullet.IsOld) 
            {
                DeleteOldActiveBullet(bullet);
                return;
            }

            _pool.Return(bullet);
        }

        private void DeleteOldActiveBullet(BaseBullet bullet) 
        {
            _activeBullet.Remove(bullet);
            bullet.Dispose();
        }

        private void DestroyOldBullets(List<BaseBullet> oldBulletsinPool, List<BaseBullet> oldBulletsActive)
        {
            foreach (BaseBullet bullet in oldBulletsinPool)
            {
                if (bullet != null)
                    bullet.Dispose();   
            }

            foreach(BaseBullet bullet in oldBulletsActive) 
            {
                bullet.SetOldBulletOld();
            }

            oldBulletsinPool.Clear();
        }

        private void DeactivateBullet(BaseBullet bullet) 
        {
            _activeBullet.Remove(bullet);
            bullet.Deactivate();
        }

        public void CreateProjectile(Vector3 position, Vector3 direction)
        {
            var bullet = _pool.Get();
            _activeBullet.Add(bullet);
            bullet.Init(position, direction, bullet => ReturnBulletToPool(bullet), _bulletData, _damageModificator.Value, _criticalChanceModificator.Value, _criticalDamageMultiplier.Value, _bulletSize.Value);
        }
    }
}
