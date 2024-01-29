using TandC.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.Gameplay 
{
    public class PlayerHealthView : MonoBehaviour, IEventReceiver<PlayerHealthChangeEvent>
    {
        private Image _healtBar;
        [SerializeField] private EventBusHolder _eventBusHolder;

        private void OnEnable()
        {
            _eventBusHolder.EventBus.Register(this as IEventReceiver<PlayerHealthChangeEvent>);

        }

        private void OnDisable()
        {
            _eventBusHolder.EventBus.Unregister(this as IEventReceiver<PlayerHealthChangeEvent>);
        }

        public UniqueId Id { get; } = new UniqueId();

        public void OnEvent(PlayerHealthChangeEvent @event)
        {
            _healtBar.fillAmount = @event.CurrentHealth / @event.MaxHealth;
        }
    }
}

