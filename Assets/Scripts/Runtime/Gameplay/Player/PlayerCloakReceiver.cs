using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class PlayerCloakReceiver : IEventReceiver<CloakingEvent>
    {
        private readonly Collider2D _playerCollider;
        private readonly SpriteRenderer _playerSpriteModel;

        public UniqueId Id { get; } = new UniqueId();

        public PlayerCloakReceiver(Collider2D playerCollider, SpriteRenderer playerSpriteModel) 
        {
            _playerCollider = playerCollider;
            _playerSpriteModel = playerSpriteModel;
            Debug.LogError(_playerSpriteModel.gameObject.name);
            RegisterEvent();
        }

        public void Dispose() 
        {
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CloakingEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CloakingEvent>);
        }

        public void OnEvent(CloakingEvent @event)
        {
            if (@event.IsActive)
                ActivateCloak();
            else if (!@event.IsActive)
                DeactivateCloak();
        }

        private void ActivateCloak() 
        {
            _playerCollider.enabled = false;
            SetPlayerAlpha(0.5f);
        }

        private void SetPlayerAlpha(float alpha) 
        {
            Color currentColor = _playerSpriteModel.color;
            _playerSpriteModel.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        }

        private void DeactivateCloak() 
        {
            _playerCollider.enabled = true;
            SetPlayerAlpha(1f);
        }
    }
}
