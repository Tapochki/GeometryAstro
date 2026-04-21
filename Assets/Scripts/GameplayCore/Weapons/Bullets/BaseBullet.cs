using System;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Abstract base class for all projectile bullets.
    /// Handles lifetime, damage, crit, size and pooling.
    ///
    /// Enemy GameObjects must implement IDamageable to receive damage.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseBullet : MonoBehaviour, ITickable
    {
        protected Action<BaseBullet> _returnToPoolCallback;
        protected IMove _moveComponent;
        protected IRotation _rotationComponent;
        protected BulletData _bulletData;

        protected float _lifeTimer;
        protected float _damage;
        protected float _criticalChance;
        protected float _criticalMultiplier;

        private void Awake()
        {
            InitComponents();
        }

        protected virtual void InitComponents()
        {
            _moveComponent = new MoveInDirectionComponent(GetComponent<Rigidbody2D>());
            _rotationComponent = new OnTargetRotateComponent(transform);
        }

        public virtual void Init(
            Vector3 startPosition,
            Vector3 targetPosition,
            Action<BaseBullet> returnToPoolCallback,
            BulletData bulletData,
            float damageMultiplier,
            float critChance,
            float critDamageMultiplier,
            float bulletSizeMultiplier)
        {
            transform.position = startPosition;
            _rotationComponent?.SetRotation(targetPosition);

            _returnToPoolCallback = returnToPoolCallback;
            _bulletData = bulletData;
            _lifeTimer = bulletData.BulletLifeTime;

            _damage = bulletData.BaseDamage * damageMultiplier;
            _criticalChance = bulletData.BasicCriticalChance + critChance;
            _criticalMultiplier = bulletData.BasicCriticalMultiplier + critDamageMultiplier;

            float size = bulletData.BasicBulletSize * bulletSizeMultiplier;
            transform.localScale = new Vector3(size, size, size);
        }

        public void Activate() => gameObject.SetActive(true);
        public void Deactivate() => gameObject.SetActive(false);

        public virtual void Tick()
        {
            _moveComponent?.Move(Vector2.up, _bulletData.BulletSpeed);
            TickLifeTimer();
        }

        protected virtual void TickLifeTimer()
        {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0f)
                ReturnToPool();
        }

        protected virtual void ReturnToPool()
        {
            _returnToPoolCallback?.Invoke(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(_damage, _criticalChance, _criticalMultiplier);
                OnBulletHit();
            }
        }

        protected virtual void OnBulletHit()
        {
            ReturnToPool();
        }

        public void Dispose()
        {
            _moveComponent = null;
            _rotationComponent = null;
            Destroy(gameObject);
        }
    }
}
