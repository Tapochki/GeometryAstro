using System;
using TandC.Data;
using TandC.Settings;
using TandC.Utilities;
using UnityEngine;

namespace TandC.Gameplay
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected Bullet _bulletPrefab;
        [SerializeField] protected GameplayData _gameplayData;
        [SerializeField] protected Transform _bulletParent;
        [SerializeField] protected WeaponType _weaponType;

        protected ObjectPool<Bullet> _bulletPool;
        protected BulletData _currentBulletData;
        protected WeaponData _currentWeaponData;

        protected float _shootDeleyTimer;
        protected bool _isReloaded;
        
        protected abstract void InitializeBulletPrefab();

        protected virtual void ActivateWeapon()
        {
            gameObject.SetActive(true);
            InitializeBulletPrefab();
            GetDatas();
            DiscardTimer();
        }

        private void GetDatas()
        {
            _currentBulletData = _gameplayData.GetBulletByType(_weaponType);
            _currentWeaponData = _gameplayData.GetWeaponByType(_weaponType);
            //TODO Мне вот эта дичь чет совсем не нра что что и через геймплей дату мы гетаем еще и булет и веапон дата, мб мы их совместим но это после того как конфиги настроим нормально но пока так
        }

        protected virtual void Update()
        {
            Reloading();
        }

        protected void Reloading() 
        {
            _shootDeleyTimer -= Time.deltaTime;
            if (_shootDeleyTimer <= 0)
            {
                _isReloaded = true;
            }
        }

        protected virtual void GetReadyBullet(ShootDirection shotDirection) 
        {
            Bullet bullet = _bulletPool.Get();
            bullet.Init(shotDirection.StartPosition.position, shotDirection.DirectionPosition.position, ReturnToPool, _currentBulletData, _currentWeaponData.baseDamage);
            bullet.Activate();
        }

        protected virtual void StartReload()
        {
            DiscardTimer();
            _isReloaded = false;
        }

        private void DiscardTimer() 
        {
            _shootDeleyTimer = _shootDeleyTimer = _currentWeaponData.shootDeley;
        }

        protected abstract void Shoot();

        protected Bullet Preload() => Instantiate(_bulletPrefab, _bulletParent);

        protected void GetReadyBullet(Bullet bullet) { }

        protected void BackEnemyToPool(Bullet bullet) {bullet.gameObject.SetActive(false);}

        protected void ReturnToPool(Bullet bullet)
        {
            _bulletPool.Return(bullet);
        }

        [Serializable]
        protected class ShootDirection
        {
            [SerializeField] private Transform _startPosition;
            [SerializeField] private Transform _directionPosition;

            public Transform StartPosition { get { return _startPosition; } }
            public Transform DirectionPosition { get { return _directionPosition; } }
        }
    }
}

