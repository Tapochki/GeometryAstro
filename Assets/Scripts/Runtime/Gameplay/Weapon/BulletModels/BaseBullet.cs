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

        public virtual void Init(Vector2 startPosition, Vector2 target, Action<BaseBullet> bulletBackToPoolEvent, BulletData bulletData, float damage)
        {
            transform.position = startPosition;
            _moveComponent = new MoveInDirectionComponent(GetComponent<Rigidbody2D>());
            _rotationComponent = new OnTargetRotateComponte(transform, target);

            _bulletBackToPoolEvent = bulletBackToPoolEvent;
            _bulletData = bulletData;
            _lifeTimer = bulletData.bulletLifeTime;
            _damage = damage;
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
                enemy.TakeDamage(_damage);
                BulletHit();
            }
        }

        protected virtual void BulletHit() => Dispose();
    }
}
