using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ProjectileFactory : IProjectileFactory
    {
        private ObjectPool<Bullet> _pool;
        private readonly Transform _projectileParent;
        private readonly BulletData _bulletData;

        public ProjectileFactory(BulletData bulletData, Transform projectileParent)
        {
            _bulletData = bulletData;
            _projectileParent = projectileParent;

            InitializePools();
        }

        private void InitializePools()
        {
            _pool = new ObjectPool<Bullet>(
                    preloadFunc: () => CreateBullet(),
                    getAction: bullet => bullet.Activate(),
                    returnAction: bullet => bullet.DiActivate(),
                    20);
        }

        private void HandleBulletEnd(Bullet bullet)
        {
            _pool.Return(bullet);
        }

        private Bullet CreateBullet()
        {
            var bulletObj = MonoBehaviour.Instantiate(_bulletData.BulletObject, _projectileParent);
            return bulletObj.GetComponent<Bullet>();
        }

        public void CreateProjectile(Vector2 position, Vector2 direction, BulletData data, float damage)
        {
            var bullet = _pool.Get();
            bullet.Init(position, direction, HandleBulletEnd, data, damage);
        }
    }
}
