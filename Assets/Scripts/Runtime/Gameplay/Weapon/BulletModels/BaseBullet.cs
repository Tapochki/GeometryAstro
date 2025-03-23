using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public abstract class BaseBullet : MonoBehaviour
    {
        protected Action<BaseBullet> _bulletBackToPoolEvent;
        protected IMove _moveComponent;
        protected IRotation _rotationComponent;
        protected BulletData _bulletData;

        protected float _lifeTimer;
        protected float _damage;
        protected float _criticalChance;
        protected float _criticalMultiplier;

        private void Awake()
        {
            _moveComponent = new MoveInDirectionComponent(GetComponent<Rigidbody2D>());
            _rotationComponent = new OnTargetRotateComponte(transform);
        }

        public virtual void Init(Vector3 startPosition, Vector3 target, Action<BaseBullet> bulletBackToPoolEvent, BulletData bulletData, 
            float damageModificatorMultiplier, float criticalChanceModificator, float criticalDamageMultiplier,
            float bulletSizeMultiplier)
        {
            transform.position = startPosition;

            _rotationComponent.SetRotation(target);

            _bulletBackToPoolEvent = bulletBackToPoolEvent;
            _bulletData = bulletData;
            _lifeTimer = bulletData.bulletLifeTime;

            CalculateDamage(damageModificatorMultiplier);
            SetBulletSize(bulletSizeMultiplier);
            CalculateCriticalChance(criticalChanceModificator);
            CalculateCriticalMultiplier(criticalDamageMultiplier);
        }

        private void CalculateCriticalChance(float criticalChanceModificator) 
        {
            _criticalChance = _bulletData.BasicCriticalChance + criticalChanceModificator;

        }

        private void CalculateCriticalMultiplier(float criticalDamageMultiplier)
        {
            _criticalMultiplier = _bulletData.BasicCriticalMultiplier + criticalDamageMultiplier;
        }

        private void CalculateDamage(float damageModificatorMultiplier) 
        {
            _damage = _bulletData.baseDamage * damageModificatorMultiplier;
        }

        private void SetBulletSize(float bulletSizeMultiplier) 
        {
            transform.localScale = new Vector2(_bulletData.BasicBulletSize * bulletSizeMultiplier, _bulletData.BasicBulletSize * bulletSizeMultiplier);
        }

        public void Activate() => gameObject.SetActive(true);
        public void Deactivate() => gameObject.SetActive(false);

        protected virtual void Update()
        {
            _moveComponent.Move(Vector2.up, _bulletData.BulletSpeed);
            LifeTimer();
        }

        protected virtual void LifeTimer()
        {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer < 0)
            {
                Dispose();
            }
        }

        protected virtual void Dispose()
        {
            _bulletBackToPoolEvent?.Invoke(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {               
                enemy.TakeDamage(_damage, _criticalChance, _criticalMultiplier);
                BulletHit();
            }
        }

        protected virtual void BulletHit() => Dispose();
    }
}
