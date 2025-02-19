using System.Collections.Generic;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class DefaultWeapon : Weapon
    {
        private const int DEFAULT_BULLET_PRELOAD_COUNT = 200;

        [SerializeField] private GameObject _shootReloadDetector;
        [SerializeField] private List<ShootDirection> _shootDirections;
        [SerializeField] private LineEnemyDetector _enemyLineDetector;

        private int _activatedDirection;

        private void Start()
        {
            ActivateWeapon();
        }

        protected override void ActivateWeapon()
        {
            base.ActivateWeapon();
            _activatedDirection = 1;
        }

        protected override void InitializeBulletPrefab()
        {
            _bulletPool = new ObjectPool<Bullet>(Preload, GetReadyBullet, BackEnemyToPool, DEFAULT_BULLET_PRELOAD_COUNT);
        }

        protected override void Update()
        {
            base.Update();
            if (_enemyLineDetector.IsEnemyOnLine && _isReloaded) 
            {
                Shoot();
            }
        }

        protected override void Shoot()
        {
            for(int i = 0; i < _activatedDirection; i++) 
            {
                GetReadyBullet(_shootDirections[i]);
            }
            StartReload();
        }
    }
}

