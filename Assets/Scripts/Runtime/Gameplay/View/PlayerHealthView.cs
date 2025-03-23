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

        public UniqueId Id { get; } = new UniqueId();

        private void RegisterEvent()
        {
           EventBusHolder.EventBus.Register(this as IEventReceiver<PlayerHealthChangeEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<PlayerHealthChangeEvent>);
        }

        public void OnEvent(PlayerHealthChangeEvent @event)
        {
            _healthBar.fillAmount = @event.CurrentHealth / @event.MaxHealth;
            _healthText.text = $"{@event.CurrentHealth}/{@event.MaxHealth}";
        }

        private void OnEnable()
        {
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnregisterEvent();
        }
    }
}

