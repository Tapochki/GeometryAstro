﻿using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using UniRx;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private Transform _skillTransform;

        private IShield _shield;

        public Transform SkillTransform { get { return _skillTransform; } }

        private IReadableModificator _moveSpeed;
        private IGameplayInputHandler _inputHandler;
        private TickService _tickService;

        private PlayerData _playerData;

        private IMove _moveComponent;
        private IRotation _mainRotateComponent;
        private IHealth _healthComponent;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<int, float, float, bool> _onHealthChageEvent;
        private Action<bool> _onPlayerDieEvent;

        private PlayerDieEvent _playerDieEvent;

        private LevelModel _levelModel;

        private ModificatorContainer _modificatorContainer;
        private PlayerTeleportationComponent _playerTeleportationComponent;

        private HealthRegenerator _healthRegenerator;

        private ItemPickUper _itemPickuper;

        private PlayerCloakReceiver _playerCloak;

        private bool _isPlayerCanMove;

        public bool IsDead { get; private set; }

        [Inject]
        private void Construct(IGameplayInputHandler inputHandler, ModificatorContainer modificatorContainer, TickService tickService)
        {
            _inputHandler = inputHandler;
            _tickService = tickService;
            _modificatorContainer = modificatorContainer;
        }

        public void Init(PlayerData playerData)
        {
            _playerData = playerData;
            IsDead = false;
            _isPlayerCanMove = true;
            InitLevelModel();
            InitPlayerHealthComponent();
            InitPlayerMoveComponent();
            InitRotateComponent();
            InitPickUpper();
            InitPlayerTeleportationComponent();

            _tickService.RegisterFixedUpdate(FixedTick);
        }

        private void InitPlayerTeleportationComponent()
        {
            _playerTeleportationComponent = new PlayerTeleportationComponent(transform, _healthComponent);
        }

        private void InitLevelModel()
        {
            _levelModel = new LevelModel();
            _levelModel.Init(_modificatorContainer.GetModificator(Settings.ModificatorType.ReceivedExperience));
        }

        private void InitPlayerMoveComponent()
        {
            _moveSpeed = _modificatorContainer.GetModificator(Settings.ModificatorType.SpeedMoving);
            _moveComponent = new MoveComponent(gameObject.GetComponent<Rigidbody2D>());
        }

        private void InitPlayerHealthComponent()
        {
            _onHealthChageEvent += (currentHealth, maxHealth, ChangedValue, isDamageOrHeal) => EventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(currentHealth, maxHealth, ChangedValue, isDamageOrHeal));
            _onPlayerDieEvent += (isKilled) => EventBusHolder.EventBus.Raise(new PlayerDieEvent());

            _healthComponent = new ModifiableHealth(_modificatorContainer.GetModificator(Settings.ModificatorType.MaxHealth).Value,
                _onPlayerDieEvent, _onHealthChageEvent,
                _modificatorContainer.GetModificator(Settings.ModificatorType.MaxHealth),
                _modificatorContainer.GetModificator(Settings.ModificatorType.Armor));

            _healthRegenerator = new HealthRegenerator(_healthComponent, _modificatorContainer.GetModificator(Settings.ModificatorType.HealtRestoreCount));
        }

        private void InitPickUpper()
        {
            _itemPickuper = FindAnyObjectByType<ItemPickUper>();
            _itemPickuper.SetModificator(_modificatorContainer.GetModificator(Settings.ModificatorType.PickUpRadius));
        }

        private void InitRotateComponent()
        {
            _mainRotateComponent = new PlayerRotateComponent(_bodyTransform);
        }

        private void FixedTick()
        {
            _healthRegenerator.Tick();
            if (!_isPlayerCanMove)
                return;
            _moveComponent.Move(_inputHandler.MoveDirection, _moveSpeed.Value);
            if (_inputHandler.RotationDirection != Vector2.zero)
            {
                _mainRotateComponent.SetRotation(_inputHandler.RotationDirection);
            }
            else if (_inputHandler.MoveDirection != Vector2.zero)
            {
                _mainRotateComponent.SetRotation(_inputHandler.MoveDirection);
            }

            if (_mainRotateComponent != null)
            {
                _mainRotateComponent.Update();
            }
        }

        public void TakeDamage(float damage)
        {
            bool isDamageAbsorb = false;
            if (_shield != null) 
            {
                isDamageAbsorb = _shield.TryAbsorbDamage();
            }
            if (!isDamageAbsorb)
                _healthComponent.TakeDamage(damage);
        }

        public void SetShield(IShield shield) 
        {
            _shield = shield;
        }

        public void PlayerEnable()
        {
            IsDead = false;
            EventBusHolder.EventBus.Raise(new PauseGameEvent(false));
            gameObject.SetActive(true);
        }

        public void PlayerDisable()
        {
            IsDead = true;
            EventBusHolder.EventBus.Raise(new PauseGameEvent(true));
            gameObject.SetActive(false);
        }

        public IDashMove RegisterDashSkill(IReadableModificator dashModificator) 
        {
            MoveDashComponent move = new MoveDashComponent(gameObject.GetComponent<Rigidbody2D>(), gameObject.GetComponent<Collider2D>(), dashModificator, _moveComponent);
            _moveComponent = move;
            return move;
        }

        public void RegisterCloak() 
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            _playerCloak = new PlayerCloakReceiver(gameObject.GetComponent<Collider2D>(), spriteRenderer);
        }

        public void SetRocketAmmo(IReadOnlyReactiveProperty<int> ammoCount, IReadOnlyReactiveProperty<int> maxAmmoCount) 
        {
            _itemPickuper.SetCanPickUpRocket(ammoCount, maxAmmoCount);
        }

        public void SetPlayerCanMove(bool value) 
        {
            _isPlayerCanMove = value;
        }

        private void OnDestroy()
        {
            _playerTeleportationComponent.Dispose();
            _playerCloak?.Dispose();
        }
    }
}