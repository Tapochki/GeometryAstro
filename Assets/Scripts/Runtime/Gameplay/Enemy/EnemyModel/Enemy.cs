using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class Enemy : MonoBehaviour, IEnemyDamageable
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _modelViewRenderer;
        [SerializeField] private Rigidbody2D _rigidbody;

        private Transform _target;
        private IMove _moveComponent;
        private IRotation _rotationComponent;
        private IHealth _healthComponent;
        private AttackComponent _attackComponent;
        private Action<Enemy, bool> _onDeathEvent;

        public EnemyData EnemyData { get; private set; }

        private void Update()
        {
            if (_moveComponent != null)
            {
                _moveComponent.Move(_target.position, EnemyData.movementSpeed);
            }

            if (_rotationComponent != null)
            {
                _rotationComponent.Update();
            }

            if (_attackComponent != null)
            {
                _attackComponent.Update();
            }
        }

        public void Initialize(EnemyData data, Transform target, Action<Enemy, bool> onDeathEvent)
        {
            EnemyData = data;
            _target = target;
            _onDeathEvent = onDeathEvent;

            _modelViewRenderer.sprite = data.mainSprite;
            SetupHealthComponent();
        }

        public void SetupHealthComponent()
        {
            _healthComponent = new BaseHealthComponent(
                EnemyData.health,
                ProccesingEnemyDeath);
        }

        public void ProccesingEnemyDeath(bool isKilled = true)
        {
            _onDeathEvent?.Invoke(this, isKilled);
        }

        public void ConfigureComponents(IMove moveComponent, IRotation rotationComponent, AttackComponent attackComponent)
        {
            _moveComponent = moveComponent;
            _rotationComponent = rotationComponent;
            _attackComponent = attackComponent;
        }

        public void TakeDamage(float damageValue, float criticalChance, float criticalDamageMultiplier)
        {
            bool isCriticalHit = UnityEngine.Random.Range(1f, 100f) <= criticalChance;
            float finalDamage = isCriticalHit ? damageValue * criticalDamageMultiplier : damageValue;
            if (isCriticalHit)
            {
                //Send damage value to vfx 
            }

            _healthComponent.TakeDamage(finalDamage);
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
    }
}
