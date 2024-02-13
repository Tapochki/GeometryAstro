using System;
using TandC.Data;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class Bullet : MonoBehaviour
    {
        private Action<Bullet, bool> _bulletBackToPoolEvent;

        private IMove _moveComponent;
        private IRotation _rotationComponent;
        private BulletData _bulletData;

        private float _lifeTimer;
        private float _damage;

        public void Init(Vector2 startPosition, Vector2 target, Action<Bullet, bool> bulletBackToPoolEvent, BulletData bulletData, float damage)
        {
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
            if(_lifeTimer < 0 ) 
            {
                Dispose();
            }
        }

        private void Dispose() 
        {
            _bulletBackToPoolEvent?.Invoke(this, false);
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
                BulletDie();
            }
        }
    }

    public class RocketBullet : Bullet 
    {
        protected virtual void BulletDie()
        {
            base.BulletDie();
            //TODO add spawn blow to kill enemy
        }
    }

    public class BulletWithHealth : Bullet
    {
        protected virtual void BulletDie()
        {
            base.BulletDie();
            //TODO minusHealth before Die
        }
    }
}


