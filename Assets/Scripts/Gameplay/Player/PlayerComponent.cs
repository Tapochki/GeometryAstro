using ChebDoorStudio.Gameplay.Items.Base;
using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.SceneSystems;
using ChebDoorStudio.ScriptableObjects;
using ChebDoorStudio.UI.Views;
using ChebDoorStudio.Utilities;
using System;
using UnityEngine;
using Zenject;
using static ChebDoorStudio.Gameplay.Player.PlayerComponent;

namespace ChebDoorStudio.Gameplay.Player
{
    public sealed class PlayerComponent : MonoBehaviour
    {
        public event Action OnPlayerDeathEvent;

        private Transform _selfTransform;

        private GameObject _particleDeathPrefab;

        private Transform _model;
        private SpriteRenderer _modelView;
        private OnBehaviourHandler _playerCollision;

        private PlayerMovement _movement;
        private PlayerPickupHandler _pickupHandler;
        private PlayerSkinChanger _playerSkinChanger;

        private InputsSystem _inputSystem;
        private GameStateSystem _gameStateSystem;
        private InitialGameData _initialGameData;
        private SoundSystem _soundSystem;
        private LoadObjectsSystem _loadObjectsSystem;
        private DataSystem _dataSystem;
        private ItemSpawnSystem _itemSpawnSystem;
        private VaultSystem _vaultSystem;
        private UISystem _uiSystem;

        private float _faceDirection = -1.0f;
        private Vector3 _initialFaceDirection = Vector3.one;

        [Inject]
        public void Construct(InputsSystem inputsSystem, GameStateSystem gameStateSystem,
                                InitialGameData initialGameData, SoundSystem soundSystem,
                                LoadObjectsSystem loadObjectsSystem, DataSystem dataSystem,
                                ItemSpawnSystem itemSpawnSystem, VaultSystem vaultSystem,
                                UISystem uiSystem)
        {
            _inputSystem = inputsSystem;
            _gameStateSystem = gameStateSystem;
            _initialGameData = initialGameData;
            _soundSystem = soundSystem;
            _loadObjectsSystem = loadObjectsSystem;
            _dataSystem = dataSystem;
            _itemSpawnSystem = itemSpawnSystem;
            _vaultSystem = vaultSystem;
            _uiSystem = uiSystem;

            _inputSystem.OnMovementDirectionUpdatedEvent += OnMovementDirectionUpdatedEventHandler;
            _gameStateSystem.OnGameplayStopedEvent += OnGameplayStopedEventHandler;
        }

        [Inject]
        public void Initialize()
        {
            _selfTransform = this.transform;

            _model = transform.Find("Model").transform;
            _modelView = _model.Find("ModelView").GetComponent<SpriteRenderer>();
            _playerCollision = _model.GetComponent<OnBehaviourHandler>();

            _particleDeathPrefab = _loadObjectsSystem.GetObjectByPath<GameObject>("Prefabs/Particle_Death");

            _movement = new PlayerMovement(_selfTransform, _initialGameData);
            _pickupHandler = new PlayerPickupHandler(_itemSpawnSystem, _vaultSystem);
            _playerSkinChanger = new PlayerSkinChanger(_uiSystem, _modelView);

            _modelView.sprite = _initialGameData.playerSkins.Find(x => x.type == _dataSystem.SelectedPlayerSkinData.skin).skin;

            _playerCollision.OnCollision2DEnterEvent += OnCollision2DEnterEventHandler;
            _playerCollision.OnTrigger2DEnterEvent += OnTrigger2DEnterEventHandler;
        }

        private void FixedUpdate()
        {
            if (!_gameStateSystem.GameStarted)
            {
                return;
            }

            if (_movement != null)
            {
                _movement.FixedUpdate();
            }
        }

        private void OnCollision2DEnterEventHandler(Collision2D collision)
        {
            _soundSystem.PlaySound(Settings.Sounds.PlayerDeath);
            Instantiate(_particleDeathPrefab, _model);
            OnPlayerDeathEvent?.Invoke();
        }

        private void OnTrigger2DEnterEventHandler(Collider2D collider)
        {
            _soundSystem.PlaySound(Settings.Sounds.CoinPickUp);
            collider.GetComponent<ItemBase>().Pickup();
        }

        private void OnMovementDirectionUpdatedEventHandler()
        {
            if (!_gameStateSystem.GameStarted)
            {
                return;
            }

            _movement.UpdateMovementDirection();

            _initialFaceDirection.y *= _faceDirection;

            UpdateFaceLookDirection();
        }

        private void UpdateFaceLookDirection()
        {
            _model.transform.localScale = _initialFaceDirection;
        }

        private void OnGameplayStopedEventHandler()
        {
            _movement.ResetRotation();
            _initialFaceDirection = Vector3.one;
            UpdateFaceLookDirection();
        }

        internal class PlayerSkinChanger
        {
            private SpriteRenderer _modelView;

            private UISystem _uiSystem;

            public PlayerSkinChanger(UISystem uiSystem, SpriteRenderer modelView)
            {
                _uiSystem = uiSystem;
                _modelView = modelView;

                _uiSystem.GetView<ViewShopPage>().OnPlayerSkinChangeEvent += (Sprite skin) => _modelView.sprite = skin;
            }
        }
    }
}