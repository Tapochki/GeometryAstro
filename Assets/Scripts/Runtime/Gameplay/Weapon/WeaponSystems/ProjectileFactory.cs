using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ProjectileFactory : IProjectileFactory
    {
        private ObjectPool<BaseBullet> _pool;
        private Transform _projectileParent;
        private readonly BulletData _bulletData;
        private readonly int _startBulletPreloadCount;

        private IReadableModificator _damageModificator;

        public ProjectileFactory(BulletData bulletData, int startBulletPreloadCount, IReadableModificator damageModificator)
        {
            _damageModificator = damageModificator;
            _bulletData = bulletData;
            _startBulletPreloadCount = startBulletPreloadCount;
            CreateBulletParent();
            InitializePool();
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
                returnAction: bullet => bullet.Deactivate(),
                _startBulletPreloadCount);
        }

        private BaseBullet CreateBullet()
        {
            var bulletObj = Object.Instantiate(_bulletData.BulletObject, _projectileParent);
            return bulletObj.GetComponent<BaseBullet>();
        }

        public void CreateProjectile(Vector2 position, Vector2 direction, float damage)
        {
            var bullet = _pool.Get();
            bullet.Init(position, direction, b => _pool.Return(b), _bulletData, damage * _damageModificator.Value);
        }
    }
}
