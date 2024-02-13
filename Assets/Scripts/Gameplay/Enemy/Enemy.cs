using System;
using TandC.Data;
using TandC.EventBus;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace TandC.Gameplay 
{
    public class Enemy : MonoBehaviour
    {
        protected Transform _target;
        protected Vector2 _rotateDirection;
        protected IMove _moveComponent;
        protected IRotation _rotationComponent;
        protected HealthComponent _healthComponent;
        protected AttackComponent _attackComponent;
        protected EnemyData _data;
        protected Action<Enemy> _enemyBackToPoolEvent;
        private ItemSpawner _itemSpawner;

        [Inject]
        private void Construct(ItemSpawner itemSpawner)
        {
            Debug.LogError(12);
            _itemSpawner = itemSpawner;
        }
        public void SetData(EnemyData data) 
        {
            _data = data;
        }

        public void SetBackToPoolEvent(Action<Enemy> enemyBackToPoolEvent) 
        {
            _enemyBackToPoolEvent = enemyBackToPoolEvent;
        }

        public void SetTargetDirection(Transform target) 
        {
            _target = target;
        }

        public void SetTargetRotation(Vector2 rotateDirection)
        {
            _rotateDirection = rotateDirection;
        }

        public void SetMovementComponent(IMove moveComponent)
        {
            _moveComponent = moveComponent;
        }

        public void SetRotationComponent(IRotation rotationComponent)
        {
            _rotationComponent = rotationComponent;
        }

        public void SetHealthComponent(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
        }

        public void SetAttackComponent(AttackComponent healthComponent)
        {
            _attackComponent = healthComponent;
        }

        public void Update()
        {
            _moveComponent.Move(_target.position, _data.movementSpeed);
            _rotationComponent.Rotation(_rotateDirection);
            _attackComponent.Update();
        }

        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                _attackComponent.SubscribePlayer(player);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                _attackComponent.UnSubscribePlayer();
            }
        }

        public void ProccesingEnemyDeath() 
        {
            //TODO start VFX effect
            //TODO spawn item afterDeath
            _enemyBackToPoolEvent?.Invoke(this);
            _itemSpawner.DropItem(_data.droperType, gameObject.transform.position);
        }
    }
}
