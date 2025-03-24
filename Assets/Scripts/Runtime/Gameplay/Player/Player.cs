using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _bodyTransform;

        private IReadableModificator _moveSpeed;
        private IGameplayInputHandler _inputHandler;
        private TickService _tickService;

        private PlayerData _playerData;

        private IMove _moveComponent;
        private IRotation _mainRotateComponent;
        private IHealth _healthComponent;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<int, float> _onHealthChageEvent;
        private Action<bool> _onPlayerDieEvent;

        private PlayerDieEvent _playerDieEvent;

        private LevelModel _levelModel;

        private ModificatorContainer _modificatorContainer;
        private PlayerTeleportationComponent _playerTeleportationComponent;

        private HealthRegenerator _healthRegenerator;

        private ItemPickUper _itemPickuper;

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
            _onHealthChageEvent += (currentHealth, maxHealth) => EventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(currentHealth, maxHealth));
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
            _healthRegenerator.Tick();
        }

        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
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

        private void OnDestroy()
        {
            _playerTeleportationComponent.Dispose();
        }
    }
}