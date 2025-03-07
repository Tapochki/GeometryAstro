using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class Bullet : MonoBehaviour
    {
        private Action<Bullet> _bulletBackToPoolEvent;

        private IMove _moveComponent;
        private IRotation _rotationComponent;
        private BulletData _bulletData;

        private float _lifeTimer;
        private float _damage;

        public void Init(Vector2 startPosition, Vector2 target, Action<Bullet> bulletBackToPoolEvent, BulletData bulletData, float damage)
        {
            Debug.LogError(startPosition);
            gameObject.transform.position = startPosition;

            _moveComponent = new MoveInDirectionComponent(gameObject.GetComponent<Rigidbody2D>());
            _rotationComponent = new OnTargetRotateComponte(gameObject.transform, target);

            _bulletBackToPoolEvent = bulletBackToPoolEvent;
            _bulletData = bulletData;
            _lifeTimer = bulletData.bulletLifeTime;
            _damage = damage;
        }

        public void Activate()
        {
            Debug.LogError(12);
            gameObject.SetActive(true);
        }

        private void Update()
        {
            _moveComponent.Move(Vector2.up, _bulletData.BulletSpeed);
            LifeTimer();
        }

        private void LifeTimer()
        {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer < 0)
            {
                Dispose();
            }
        }

        public void DiActivate() 
        {
            gameObject.SetActive(false);
        }

        private void Dispose()
        {
            _bulletBackToPoolEvent?.Invoke(this);
        }

        protected virtual void BulletDie()
        {
            Dispose();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
                //BulletDie();
            }
        }
    }
}
