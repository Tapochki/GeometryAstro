using TandC.GeometryAstro.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.Gameplay
{
    public class PlayerHealthView : MonoBehaviour, IEventReceiver<PlayerHealthChangeEvent>
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Animator _lowHealthAnimator;
        [SerializeField] private Animator _damageIndicatorAnimator;

        public UniqueId Id { get; } = new UniqueId();

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this);
        }

        public void OnEvent(PlayerHealthChangeEvent @event)
        {
            int current = @event.CurrentHealth;
            float max = @event.MaxHealth;

            _lowHealthAnimator.gameObject.SetActive(false);
            if (current / max <= 0.2f)
            {
                _lowHealthAnimator.gameObject.SetActive(true);
                _lowHealthAnimator.Play("Action", -1, 0);
            }

            _damageIndicatorAnimator.gameObject.SetActive(true);
            _damageIndicatorAnimator.Play("Action", -1, 0);

            _healthBar.fillAmount = current / max;
            _healthText.text = $"{current}/{max}";
        }

        private void OnEnable()
        {
            RegisterEvent();

            _lowHealthAnimator.gameObject.SetActive(false);
            _damageIndicatorAnimator.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            UnregisterEvent();
        }
    }
}

