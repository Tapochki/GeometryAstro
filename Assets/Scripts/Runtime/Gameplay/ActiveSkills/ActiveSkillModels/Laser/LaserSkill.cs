using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace TandC.GeometryAstro.Gameplay 
{
    public class LaserSkill : IActiveSkill, IEventReceiver<DashEvent>, IEventReceiver<CloakingEvent>
    {
        public bool IsWeapon { get => true; }

        private IReloadable _reloader;
        private IReloadable _activeTimer;

        private ActiveSkillData _data;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.LaserDestroyerGun;

        private bool _isEvolved;

        private bool _isCloakActive;
        private bool _isDashActive;

        private bool _isLaserActive;

        private Action<bool> _interactableMoveInput;

        public UniqueId Id { get; } = new UniqueId();

        private string _laserAnimationName;

        private Animator _laserAnimator;

        private LaserView _laserView;
        private GameObject _laser;

        public void SetData(ActiveSkillData data)
        {
            _data = data;
        }

        public void SetReloader(IReloadable reloader, IReloadable activeTimer, SkillInputButton cloacingInputButton)
        {
            _reloader = reloader;
            _activeTimer = activeTimer;
            cloacingInputButton.Initialize(_reloader.ReloadProgress, _activeTimer.ReloadProgress, ActivateLaser);
        }

        public void OnEvent(DashEvent @event)
        {
            _isDashActive = @event.IsActive;
        }

        public void OnEvent(CloakingEvent @event)
        {
            _isCloakActive = @event.IsActive;
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<DashEvent>);
            EventBusHolder.EventBus.Register(this as IEventReceiver<CloakingEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<DashEvent>);
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CloakingEvent>);
        }

        public void SetInteractableMoveAction(Action<bool> action) 
        {
            _interactableMoveInput = action;
        }

        public void SetLaserPrefab(Transform owner, IReadableModificator damageModificator, IReadableModificator critChanceModificator, IReadableModificator critDamageMultiplierModificator) 
        {
            _laser = GameObject.Instantiate(_data._skillPrefab, owner);
            _laserView = _laser.GetComponentInChildren<LaserView>();
            _laserView.Init(_data.bulletData, damageModificator, critChanceModificator, critDamageMultiplierModificator);
            _laserAnimator = _laser.GetComponent<Animator>();
            _laserAnimationName = "LaserAnimation";
            _laser.gameObject.SetActive(false);
        }

        public void Initialization()
        {
            RegisterEvent();
        }

        private void ActivateLaser() 
        {
            if (_isCloakActive || _isDashActive || _isLaserActive)
                return;
            if (_reloader.CanAction)
            {
                _laser.gameObject.SetActive(true);
                _laserAnimator.Play(_laserAnimationName, -1, 0);
                _interactableMoveInput?.Invoke(false);
                _isLaserActive = true;
                _activeTimer.StartReload();
                EventBusHolder.EventBus.Raise(new LaserEvent(true));
            }
        }

        private void EndLaser() 
        {
            _laser.gameObject.SetActive(false);
            _interactableMoveInput?.Invoke(true);
            _isLaserActive = false;
            _reloader.StartReload();
            EventBusHolder.EventBus.Raise(new LaserEvent(false));
        }

        public void Upgrade(float value = 0)
        {
            
        }

        private void SetNewEvolvedAnimation() 
        {
            _laserAnimationName = "LaserEvolutedAnimation";
        }

        public void Evolve()
        {
            _laserView.Evolve(_data.EvolvedBulletData);
            SetNewEvolvedAnimation();
            _isEvolved = true;
        }

        public void Tick()
        {
            if(!_isLaserActive)
                _reloader.Update();

            if(_isLaserActive) 
            {
                _activeTimer.Update();
                if (_activeTimer.CanAction) 
                {
                    EndLaser();
                }
            }
        }
    }
}
