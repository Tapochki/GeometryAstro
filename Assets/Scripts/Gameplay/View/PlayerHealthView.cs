using TandC.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.Gameplay
{
    public class PlayerHealthView : MonoBehaviour, IEventReceiver<PlayerHealthChangeEvent>
    {
        [SerializeField] private Image _healtBar;
        [SerializeField] private EventBusHolder _eventBusHolder;

        private void OnEnable()
        {
            _eventBusHolder.EventBus.Register(this);
        }

        private void OnDisable()
        {
            _eventBusHolder.EventBus.Unregister(this);
        }

        public UniqueId Id { get; } = new UniqueId();

        public void OnEvent(PlayerHealthChangeEvent @event)
        {
            _healtBar.fillAmount = @event.CurrentHealth / @event.MaxHealth;
        }
    }
}