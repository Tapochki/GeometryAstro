using TandC.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TandC.Gameplay 
{
    public class PlayerHealthView : MonoBehaviour, IEventReceiver<PlayerHealthChangeEvent>
    {

        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _healthText;

        private EventBusHolder _eventBusHolder;

        [Inject]
        private void Construct(EventBusHolder eventBusHolder)
        {
            _eventBusHolder = eventBusHolder;
        }

        private void RegisterEvent()
        {
            _eventBusHolder.EventBus.Register(this as IEventReceiver<PlayerHealthChangeEvent>);
        }

        private void UnregisterEvent()
        {
            _eventBusHolder.EventBus.Unregister(this as IEventReceiver<PlayerHealthChangeEvent>);
        }

        private void OnEnable()
        {
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnregisterEvent();
        }

        public UniqueId Id { get; } = new UniqueId();

        public void OnEvent(PlayerHealthChangeEvent @event)
        {
            _healthBar.fillAmount = @event.CurrentHealth / @event.MaxHealth;
            _healthText.text = $"{@event.CurrentHealth}/{@event.MaxHealth}";
        }
    }
}

