using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DashSkill : IActiveSkill
    {
        public bool IsWeapon { get => true; }

        private IReloadable _reloader;
        private IReloadable _activeTimer;
        private ActiveSkillData _data;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.Dash;

        private IPassiveUpgradable _dashTimeUpgrade;
        private IPassiveUpgradable _dashModificatorUpgrade;

        private DashView _dashView;

        private IDashMove _dashMove;

        private bool _isEvolved;

        private bool _isDashActive;

        public void SetData(ActiveSkillData data)
        {
            _data = data;
        }

        public void SetReloader(IReloadable reloader, IReloadable activeTimer, IPassiveUpgradable dashTimeUpgrade, SkillInputButton dashInputButton)
        {
            _reloader = reloader;
            _activeTimer = activeTimer;
            _dashModificatorUpgrade = dashTimeUpgrade;
            dashInputButton.Initialize(_reloader.ReloadProgress, _activeTimer.ReloadProgress, ActivateDash);
        }

        public void SetDashComponent(IDashMove dashMove, IPassiveUpgradable dashModificatorUpgrade)
        {
            _dashMove = dashMove;
            _dashModificatorUpgrade = dashModificatorUpgrade;
        }
        public DashView InitDashObject(Transform owner) 
        {
            GameObject dash = GameObject.Instantiate(_data._skillPrefab, owner);
            _dashView = dash.GetComponentInChildren<DashView>();
            _dashView.Stop();
            return _dashView;
        }

        public void Initialization()
        {

        }

        private void ActivateDash() 
        {
            if (_isDashActive)
                return;

            if (_reloader.CanAction)
            {
                _dashView.Activete();
                _dashMove.StartDash();
                _isDashActive = true;
                _activeTimer.StartReload();
            }
        }

        private void EndDash() 
        {
            _dashView.Stop();
            _dashMove.StopDash();
            _isDashActive = false;
            _reloader.StartReload();
        }

        public void Upgrade(float value = 0)
        {
            //_dashTimeUpgrade.ApplyModifier(value);
            //_dashModificatorUpgrade.ApplyModifier(value);
        }

        public void Evolve()
        {
            _isEvolved = true;
            _dashView.Evolve(_data.EvolvedBulletData);
        }

        public void Tick()
        {
            _dashView.Tick();

            if (!_isDashActive)
                _reloader.Update();

            if(_isDashActive) 
            {
                _activeTimer.Update();
                if (_activeTimer.CanAction) 
                {
                    EndDash();
                }
            }
        }
    }
}
