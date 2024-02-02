using TandC.Data;
using UnityEngine;
using UnityEngine.UIElements;
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
        protected EnemyData _data;
        public void SetData(EnemyData data) 
        {
            _data = data;
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

        public void Update()
        {
            _moveComponent.Move(_target.position, _data.movementSpeed);
            _rotationComponent.Rotation(_rotateDirection);
        }

        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
        }
    }
}
