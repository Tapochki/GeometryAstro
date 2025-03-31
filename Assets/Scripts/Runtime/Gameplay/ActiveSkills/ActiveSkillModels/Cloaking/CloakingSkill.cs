using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CloakingSkill : IActiveSkill, IEventReceiver<DashEvent>, IEventReceiver<LaserEvent>
    {
        public bool IsWeapon { get => false; }

        private IReloadable _reloader;
        private IReloadable _activeTimer;
        private ActiveSkillData _data;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.Cloaking;

        private IPassiveUpgradable _activeCloakTimeUpgrade;

        private bool _isEvolved;

        private bool _isMaskActive;

        private bool _isDashActivated;
        private bool _isLaserActivated;

        public UniqueId Id { get; } = new UniqueId();

        public void SetData(ActiveSkillData data)
        {
            _data = data;
        }

        public void SetReloader(IReloadable reloader, IReloadable activeTimer, IPassiveUpgradable activeCloakTimeUpgrade, SkillInputButton cloacingInputButton)
        {
            _reloader = reloader;
            _activeTimer = activeTimer;
            _activeCloakTimeUpgrade = activeCloakTimeUpgrade;
            cloacingInputButton.Initialize(_reloader.ReloadProgress, _activeTimer.ReloadProgress, ActivateCloak);
        }

        public void OnEvent(DashEvent @event)
        {
            _isDashActivated = @event.IsActive;
        }

        public void OnEvent(LaserEvent @event)
        {
            _isLaserActivated = @event.IsActive;
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<DashEvent>);
            EventBusHolder.EventBus.Register(this as IEventReceiver<LaserEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<DashEvent>);
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<LaserEvent>);
        }

        public void Initialization()
        {
            RegisterEvent();
        }

        private void ActivateCloak() 
        {
            if (_isDashActivated || _isMaskActive || _isLaserActivated)
                return;
            if (_reloader.CanAction)
            {
                _isMaskActive = true;
                _activeTimer.StartReload();
                EventBusHolder.EventBus.Raise(new CloakingEvent(true, _isEvolved));
            }
        }

        private void EndCloak() 
        {
            _isMaskActive = false;
            _reloader.StartReload();
            EventBusHolder.EventBus.Raise(new CloakingEvent(false, _isEvolved));
        }

        public void Upgrade(float value = 0)
        {
            _activeCloakTimeUpgrade.ApplyModifier(value);
        }

        public void Evolve()
        {
            _isEvolved = true;
        }

        public void Tick()
        {
            if(!_isMaskActive)
                _reloader.Update();

            if(_isMaskActive) 
            {
                _activeTimer.Update();
                if (_activeTimer.CanAction) 
                {
                    EndCloak();
                }
            }
        }
    }
}
