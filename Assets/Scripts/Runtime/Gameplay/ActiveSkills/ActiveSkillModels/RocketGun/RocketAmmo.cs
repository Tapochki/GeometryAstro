using TandC.GeometryAstro.EventBus;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketAmmo : IEventReceiver<RocketAmmoItemReleaseEvent>
    {
        public IReadOnlyReactiveProperty<int> RocketCount => _rocketCount;

        private ReactiveProperty<int> _rocketCount = new ReactiveProperty<int>(10);

        public IReadOnlyReactiveProperty<int> MaxRocketCount => _maxRocketCount;

        private ReactiveProperty<int> _maxRocketCount = new ReactiveProperty<int>(10);

        public UniqueId Id { get; } = new UniqueId();

        public RocketAmmo(int startRocketCount) 
        {
            _rocketCount.Value = _maxRocketCount.Value = startRocketCount;
            RegisterEvent();
        }

        public void Dispose()
        {
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<RocketAmmoItemReleaseEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<RocketAmmoItemReleaseEvent>);
        }

        public void OnEvent(RocketAmmoItemReleaseEvent @event)
        {
            AddRockets(@event.AmmoCount);
        }

        public bool TryShoot()
        {
            if (_rocketCount.Value > 0)
            {
                _rocketCount.Value--;
                return true;
            }
            return false;
        }

        public void AddRockets(int amount = 1)
        {
            _rocketCount.Value = Mathf.Min(_maxRocketCount.Value, _rocketCount.Value + amount);
        }

        public void RemoveRockets(int amount = 1)
        {
            _rocketCount.Value = Mathf.Max(0, _rocketCount.Value - amount);
        }

        public void UpgradeRocketMaxCount(int amount = 5) 
        {
            _maxRocketCount.Value += amount;
        }
    }
}

