using TandC.GeometryAstro.EventBus;
using TMPro;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class PlayerBoundary : MonoBehaviour
    {
        [SerializeField] private GameObject _selfObject;
        [SerializeField] private GameObject _playerModel;

        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private GameObject _zoneView;

        private const float _timeToTeleportBackToZone = 5.0f;
        private float _timer;

        private bool _isActive;

        private void Awake()
        {
            _timer = _timeToTeleportBackToZone;
            _isActive = false;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(_playerModel))
            {
                if (collision.gameObject.GetComponent<Player>().IsDead)
                {
                    return;
                }

                _zoneView.SetActive(true);
                _timer = _timeToTeleportBackToZone;
                _isActive = true;
            }
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