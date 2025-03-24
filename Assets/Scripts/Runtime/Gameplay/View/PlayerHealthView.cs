using DG.Tweening;
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
        [SerializeField] private TextMeshProUGUI _changedHealthText;

        private Tween _changedHealthTween;

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
            _changedHealthTween.Complete();
            _changedHealthText.gameObject.SetActive(true);
            int current = @event.CurrentHealth;
            float max = @event.MaxHealth;

            _lowHealthAnimator.gameObject.SetActive(false);
            if (current / max <= 0.2f)
            {
                _lowHealthAnimator.gameObject.SetActive(true);
                _lowHealthAnimator.Play("Action", -1, 0);
            }

            var isSubstruct = true; // TODO FIX
            var value = 205; // TODO FIX
            string symbol = "+";
            Color color = new Color(0, 1, 0, 1);
            if (isSubstruct)
            {
                symbol = "-";
                color = new Color(1, 0, 0, 1);
            }

            _changedHealthText.text = symbol + value;
            _changedHealthText.color = color;
            _changedHealthTween = _changedHealthText.transform.DOPunchPosition(Vector2.right * 15, 0.15f);
            _changedHealthTween.onComplete = () =>
            {
                _changedHealthText.gameObject.SetActive(false);
            };

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

