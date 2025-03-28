using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class Enemy : MonoBehaviour, IEnemyDamageable, IEnemy
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
        private FreezeComponent _freezeComponent;

        public EnemyData EnemyData { get; private set; }

        private float _speedModificator;

        public void Tick()
        {
            if(_freezeComponent.IsFreeze) 
            {
                _freezeComponent.Tick();
                return;
            }
            _moveComponent.Move(_target.position, EnemyData.movementSpeed * _speedModificator);
            _rotationComponent.Update();
            _attackComponent.Update();
        }

        private void Awake() 
        {
            _freezeComponent = new FreezeComponent(gameObject.transform.Find("FreezEnemyVFX").gameObject);
        }

        public void Initialize(EnemyData data, Transform target, Action<Enemy, bool> onDeathEvent, float healthModificator, float speedModificator)
        {
            EnemyData = data;
            _target = target;
            _onDeathEvent = onDeathEvent;

            _speedModificator = speedModificator;

            _modelViewRenderer.sprite = data.mainSprite;
            SetupHealthComponent(healthModificator);
        }

        public void SetupHealthComponent(float healthModificator)
        {
            _healthComponent = new BaseHealthComponent(
                EnemyData.health * healthModificator,
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

        public void Freeze(float freezeTimer = 3f)
        {
            _freezeComponent.Freeze(freezeTimer);
        }
    }
}
