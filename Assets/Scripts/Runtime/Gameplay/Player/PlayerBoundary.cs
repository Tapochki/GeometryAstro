using TandC.GeometryAstro.EventBus;
using TMPro;
using Unity.Services.Analytics.Internal;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class PlayerBoundary : MonoBehaviour, IEventReceiver<CloakingEvent>, IEventReceiver<DashEvent>
    {
        [SerializeField] private GameObject _selfObject;
        [SerializeField] private GameObject _playerModel;

        public UniqueId Id { get; } = new UniqueId();

        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private GameObject _zoneView;

        private const float _timeToTeleportBackToZone = 5.0f;
        private float _timer;

        private bool _isActive;

        private bool _isAbility;

        private bool _playerExitBoundaryWithAbility;

        private void Awake()
        {
            _timer = _timeToTeleportBackToZone;
            _isActive = false;
            _isAbility = false;
            _playerExitBoundaryWithAbility = false;
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CloakingEvent>);
            EventBusHolder.EventBus.Register(this as IEventReceiver<DashEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CloakingEvent>);
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<DashEvent>);
        }

        public void OnEvent(DashEvent @event) 
        {
            AbilityActivated(@event.IsActive);
        }

        private void AbilityActivated(bool isActive) 
        {
            if (isActive)
            {
                PlayerEnterBoundary();
                _isAbility = true;
            }
            else if (!isActive)
            {
                if (_playerExitBoundaryWithAbility)
                {
                    PlayerExitBoundary();
                }
                _isAbility = false;
            }
        }

        public void OnEvent(CloakingEvent @event)
        {
            AbilityActivated(@event.IsActive);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(_playerModel))
            {
                if (_isAbility)
                {
                    _playerExitBoundaryWithAbility = true;
                    return;
                }
                if (collision.gameObject.GetComponent<Player>().IsDead)
                {
                    return;
                }

                PlayerExitBoundary();
            }
        }

        private void PlayerExitBoundary() 
        {
            _playerExitBoundaryWithAbility = false;
            _zoneView.SetActive(true);
            _timer = _timeToTeleportBackToZone;
            _isActive = true;
        }

        private void PlayerEnterBoundary() 
        {
            _zoneView.SetActive(false);
            _isActive = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(_playerModel))
            {
                _zoneView.SetActive(false);
                _isActive = false;
            }
        }

        private void Update()
        {
            if (_isActive)
            {
                _timer -= Time.deltaTime;
                _timeText.text = string.Format(@"{00:00.00}", _timer).Replace(',', ':');
                if (_timer <= 0)
                {
                    _isActive = false;
                    EventBusHolder.EventBus.Raise(new TeleportPlayerToTheBoundaryEvent());
                }
            }
        }
    }
}