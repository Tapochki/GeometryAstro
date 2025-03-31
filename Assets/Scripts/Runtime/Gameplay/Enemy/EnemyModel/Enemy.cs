using System;
using TandC.GeometryAstro.Data;
using UniRx;
using UnityEngine;

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
        private AttackComponent _attackComponent;
        private Action<Enemy, bool> _onDeathEvent;
        private FreezeComponent _freezeComponent;

        public EnemyData EnemyData { get; private set; }

        private float _speedModificator;

        private SpriteRenderer _enemySprite;

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
            _enemySprite = gameObject.transform.Find("ModelView").GetComponent<SpriteRenderer>();
            _freezeComponent = new FreezeComponent(gameObject.transform.Find("FreezEnemyVFX").gameObject, _rigidbody);
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

            FlashSprite();
            _healthComponent.TakeDamage(finalDamage);
        }

        private void FlashSprite()
        {
            var originalColor = _enemySprite.color;
            var transparentColor = Color.red;

            _enemySprite.color = transparentColor;

            Observable.Timer(TimeSpan.FromSeconds(_flashDuration))
                .Subscribe(_ => _enemySprite.color = originalColor)
                .AddTo(this);
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
