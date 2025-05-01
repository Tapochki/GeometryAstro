using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace TandC.GeometryAstro.Gameplay
{
    public class Enemy : MonoBehaviour, IEnemyDamageable, IEnemy
    {
        private const float _flashDuration = 0.05f;

        [Header("References")]
        [SerializeField] private SpriteRenderer _modelViewRenderer;
        [SerializeField] private Rigidbody2D _rigidbody;

        private Transform _target;
        private IMove _moveComponent;
        private IRotation _rotationComponent;
        private IHealth _healthComponent;
        private FlashSpriteComponent _flashSpriteComponent;
        private AttackComponent _attackComponent;
        private Action<Enemy, bool> _onDeathEvent;
        private FreezeComponent _freezeComponent;

        public EnemyData EnemyData { get; private set; }

        public bool IsActive { get; private set; }

        private float _speedModificator;

        private SpriteRenderer _enemySprite;

        public void Tick()
        {
            _flashSpriteComponent.Tick();
            if (_freezeComponent.IsFreeze) 
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
            _enemySprite = gameObject.transform.Find("ModelView").GetComponent<SpriteRenderer>();
            _freezeComponent = new FreezeComponent(gameObject.transform.Find("FreezEnemyVFX").gameObject, _rigidbody);
            _flashSpriteComponent = new FlashSpriteComponent(_enemySprite, _enemySprite.color);
        }

        public void Initialize(EnemyData data, Transform target, Action<Enemy, bool> onDeathEvent, float healthModificator, float speedModificator)
        {
            EnemyData = data;
            _target = target;
            _onDeathEvent = onDeathEvent;

            _speedModificator = speedModificator;

            _modelViewRenderer.sprite = data.mainSprite;
            SetupHealthComponent(healthModificator);
            IsActive = true;
        }

        public void SetupHealthComponent(float healthModificator)
        {
            _healthComponent = new BaseHealthComponent(
                EnemyData.health * healthModificator,
                ProccesingEnemyDeath);
        }

        public void ProccesingEnemyDeath(bool isKilled = true)
        {
            if (isKilled) 
            {
                ThrowDeathEffectEvent(gameObject.transform.position);
            }
            _onDeathEvent?.Invoke(this, isKilled);
            IsActive = false;
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

            ThrowDamageVFXEvent(finalDamage, gameObject.transform.position, isCriticalHit);

            _flashSpriteComponent.StartFlash();
            _healthComponent.TakeDamage(finalDamage);
        }

        private void ThrowDamageVFXEvent(float damage, Vector3 position, bool isCrit) 
        {
            EventBusHolder.EventBus.Raise(new CreateDamageEffect(damage, position, isCrit));
        }


        private void ThrowDeathEffectEvent(Vector3 position)
        {
            EventBusHolder.EventBus.Raise(new EnemyDeath(position));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_freezeComponent.IsFreeze)
                return;

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
