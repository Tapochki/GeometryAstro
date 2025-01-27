using System.Collections.Generic;
using TandC.EventBus;
using TandC.Utilities;
using UnityEngine;

namespace TandC.Gameplay
{
    public class StandartGun : Weapon, IEventReceiver<StandartGunSkillEvent>
    {
        private const int DEFAULT_BULLET_PRELOAD_COUNT = 200;

        [SerializeField] private GameObject _shootReloadDetector;
        [SerializeField] private List<ShootDirection> _shootDirections;
        [SerializeField] private LineEnemyDetector _enemyLineDetector;

        private int _activatedDirection;

        public UniqueId Id => throw new System.NotImplementedException();

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

        public void OnEvent(StandartGunSkillEvent @event)
        {
            if(_activatedDirection < 0) 
            {
                ActivateWeapon();
            }
            else 
            {
                UpgradeWeapon();
            }
        }
    }
}

