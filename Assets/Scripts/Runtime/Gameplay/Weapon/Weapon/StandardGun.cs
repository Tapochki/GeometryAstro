using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class StandardGun : MonoBehaviour, IWeapon, IDisposable
    {
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private WeaponData _data;
        private CompositeDisposable _disposables = new();
        [SerializeField]
        private Transform _startPosition;
        [SerializeField]
        private Transform _upPosition;
        [SerializeField]
        private Transform _bulletParent;
        [SerializeField]
        private WeaponConfig _config;
        private int _currentLevel = 1;

        //public StandardGun(
        //    IProjectileFactory factory,
        //    WeaponData data,
        //    IReloadable reloader,
        //    IEnemyDetector enemyDetector)
        //{
        //    _projectileFactory = factory;
        //    _reloader = reloader;
        //    _enemyDetector = enemyDetector;
        //    _config = data;
        //}

        private void Start() 
        {
            _data = _config.GetWeaponByType(WeaponType.StandardGun);
            _projectileFactory = new ProjectileFactory(_data.bulletData, _bulletParent);
            _reloader = new WeaponReloader(_data.shootDeley);
            _enemyDetector = new RaycastEnemyDetector(LayerMask.GetMask("Default"));
        }

        //private void Initialize()
        //{
        //    Observable.EveryUpdate()
        //        .Where(_ =>  _reloader.CanShoot)
        //        .Subscribe(_ => TryShoot())
        //        .AddTo(_disposables);
        //}

        private void TryShoot()
        {
            Vector2 origin = _startPosition.position;
            Vector2 direction = _upPosition.position;
            Debug.LogError("TryShoot");
            if (_enemyDetector.HasEnemyInDirection(origin, direction, _data.detectorDistance))
            {
                Debug.LogError("Shoot");

                Shoot(origin, direction);
                _reloader.StartReload();
            }
        }

        private void Shoot(Vector2 origin, Vector2 direction)
        {
            _projectileFactory.CreateProjectile(
                origin,
                direction,
                _data.bulletData,
                _data.baseDamage
            );
        }

        public void Update()
        {
            _reloader.Update();

            if (_reloader.CanShoot)
            {
                TryShoot();
            }
        }

        public void Dispose() => _disposables.Dispose();

        public void UpdateWeapon(float deltaTime)
        {
           
        }

        public void Upgrade()
        {
            
        }
    }
}


