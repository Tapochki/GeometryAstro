using System;
using TandC.GeometryAstro.EventBus;

namespace TandC.GeometryAstro.Gameplay 
{
    public class EnemyFreezeProcessor : IEventReceiver<FrozeBombReleaseEvent>
    {
        public UniqueId Id { get; } = new UniqueId();

        private Action<float> _freezeAction;

        public EnemyFreezeProcessor(Action<float> freezeAction) 
        {
            _freezeAction = freezeAction;
            RegisterEvent();
        }

        public void Dispose() 
        {
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<FrozeBombReleaseEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<FrozeBombReleaseEvent>);
        }

        public void OnEvent(FrozeBombReleaseEvent @event)
        {
            _freezeAction?.Invoke(@event.FreezeTime);
        }
    }
}

