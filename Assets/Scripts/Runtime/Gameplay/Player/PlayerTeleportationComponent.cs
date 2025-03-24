using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class PlayerTeleportationComponent : IEventReceiver<TeleportPlayerToTheBoundaryEvent>
    {
        public UniqueId Id { get; private set; } = new UniqueId();

        private Transform _playerTransform;
        private IHealth _healthComponent;

        public void OnEvent(TeleportPlayerToTheBoundaryEvent @event)
        {
            _playerTransform.position = Vector2.zero;
            _healthComponent.TakeDamage(50); // TODO FIX THIS TO ANOTHER VALUE
        }

        public PlayerTeleportationComponent(Transform playerTransform, IHealth healthComponent)
        {
            _playerTransform = playerTransform;

            EventBusHolder.EventBus.Register(this);
            _healthComponent = healthComponent;
        }

        public void Dispose()
        {
            EventBusHolder.EventBus.Unregister(this);
        }
    }
}