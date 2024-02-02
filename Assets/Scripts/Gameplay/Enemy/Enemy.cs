using TandC.Data;
using UnityEngine;

namespace TandC.Gameplay
{
    public class Enemy : MonoBehaviour
    {
        protected Player _player;
        protected IMove _moveComponent;
        protected IRotation _rotationComponent;
        protected HealthComponent _healthComponent;
        protected EnemyData _data;

        public void SetData(EnemyData data, Player player)
        {
            _player = player;
            _data = data;
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
            _moveComponent.Move(_player.transform.position, _data.movementSpeed);
            _rotationComponent.Rotation(_player.transform.position);
        }

        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
        }
    }
}