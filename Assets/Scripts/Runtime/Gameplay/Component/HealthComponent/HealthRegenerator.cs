using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class HealthRegenerator : IEventReceiver<PlayerHealReleaseEvent>, ITickable
    {
        private IHealth _health;
        private IReadableModificator _regenModificator;
        private float _timeSinceLastRegen;

        public HealthRegenerator(IHealth health, IReadableModificator regenModificator)
        {
            _health = health;
            _regenModificator = regenModificator;
            _timeSinceLastRegen = 1f;
            RegisterEvent();
        }

        public UniqueId Id { get; } = new UniqueId();

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<PlayerHealReleaseEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<PlayerHealReleaseEvent>);
        }

        public void OnEvent(PlayerHealReleaseEvent @event)
        {
            Heal(@event.HealAmount);
        }

        private void Heal(float amount) 
        {
            _health.Heal(amount);
        }

        public void Dispose() 
        {
            UnregisterEvent();
        }

        public void Tick()
        {
            if (_regenModificator.Value > 0)
            {
                _timeSinceLastRegen -= Time.deltaTime;
                if (_timeSinceLastRegen <= 0)
                {
                    Heal(_regenModificator.Value);
                    _timeSinceLastRegen = 1f;
                }
            }
        }
    }
}

