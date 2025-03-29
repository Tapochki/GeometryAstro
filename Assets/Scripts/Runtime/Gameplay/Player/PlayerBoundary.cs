using TandC.GeometryAstro.EventBus;
using TMPro;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class PlayerBoundary : MonoBehaviour, IEventReceiver<CloakingEvent>
    {
        [SerializeField] private GameObject _selfObject;
        [SerializeField] private GameObject _playerModel;

        public UniqueId Id { get; } = new UniqueId();

        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private GameObject _zoneView;

        private const float _timeToTeleportBackToZone = 5.0f;
        private float _timer;

        private bool _isActive;

        private bool _isCloakActive;

        private bool _playerExitBoundaryInCloak;

        private void Awake()
        {
            _timer = _timeToTeleportBackToZone;
            _isActive = false;
            _isCloakActive = false;
            _playerExitBoundaryInCloak = false;
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CloakingEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CloakingEvent>);
        }

        public void OnEvent(CloakingEvent @event)
        {  
            if (@event.IsActive) 
            {
                PlayerEnterBoundary();
                _isCloakActive = true;
            }
            else if (!@event.IsActive)
            {
                if (_playerExitBoundaryInCloak) 
                {
                    PlayerExitBoundary();
                }
                _isCloakActive = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(_playerModel))
            {
                Debug.LogError("PlayerBoundary OnTriggerExit2D");
                if (_isCloakActive)
                {
                    _playerExitBoundaryInCloak = true;
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
            _playerExitBoundaryInCloak = false;
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