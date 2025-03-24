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
            int current = @event.CurrentHealth;
            float max = @event.MaxHealth;

            SetChangeText(@event.IsDamageOrHeal, @event.ChangedValue);

            SetLowHealthAnimator(current, max);

            if(@event.IsDamageOrHeal)
                SetDamageIndecator();

            UpdateHealthBar(current, max);
        }

        private void SetChangeText(bool isDamageOrHeal, float value)
        {
            _changedHealthTween.Complete();
            if (value > 0)
            {
                _changedHealthText.gameObject.SetActive(true);

                string symbol = GetChangedTextSymbol(isDamageOrHeal);
                Color color = GetChangedTextColor(isDamageOrHeal);

                _changedHealthText.text = symbol + value;
                _changedHealthText.color = color;

                SetChangedHealthTween();
            }
        }

        private Color GetChangedTextColor(bool isDamageOrHeal)
        {
            return isDamageOrHeal ? new Color(1, 0, 0, 1) : new Color(0, 1, 0, 1);
        }

        private string GetChangedTextSymbol(bool isDamageOrHeal)
        {
            return isDamageOrHeal ? "-" : "+";
        }

        private void SetChangedHealthTween() 
        {
            _changedHealthTween = _changedHealthText.transform.DOPunchPosition(Vector2.right * 15, 0.15f);
            _changedHealthTween.onComplete = () =>
            {
                _changedHealthText.gameObject.SetActive(false);
            };
        }

        private void SetLowHealthAnimator(float current, float max)
        {
            _lowHealthAnimator.gameObject.SetActive(false);
            if (current / max <= 0.2f)
            {
                _lowHealthAnimator.gameObject.SetActive(true);
                _lowHealthAnimator.Play("Action", -1, 0);
            }
        }

        private void SetDamageIndecator()
        {
            _damageIndicatorAnimator.gameObject.SetActive(true);
            _damageIndicatorAnimator.Play("Action", -1, 0);
        }

        private void UpdateHealthBar(float current, float max)
        {
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

