using System;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class LaserBuilder : ActiveSkillBuilder<LaserSkill>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.LaserDestroyerGun;

        private readonly Transform _player;
        private readonly SkillInputButton _skillButton;
        private readonly Action<bool> _inpuntInteractableAction;

        public LaserBuilder(Transform player, Action<bool> action, SkillInputButton skillButton)
        {
            _player = player;
            _inpuntInteractableAction = action;
            _skillButton = skillButton;
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            IReloadable dashReload = new SkillReloader(_activeSkillData.shootDeley, reloadModificator);

            Modificator laserActiveTimeModificator = new Modificator(1, 0, true);

            IReloadable dashTimer = new SkillReloader(_config.AdditionalSkillConfig.LaserConfig.LaserActiveTime, laserActiveTimeModificator);

            _skill.SetReloader(dashReload, dashTimer, _skillButton);
        }

        private void RegisterLaser()
        {

        }

        private void SetSkillPrefab()
        {
            _skill.SetLaserPrefab(_player,
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier)
                );
        }

        private void SetInteractableMoveAction() 
        {
            _skill.SetInteractableMoveAction(_inpuntInteractableAction);
        }

        protected override void ConstructSkill()
        {
            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));
            SetInteractableMoveAction();
            SetSkillPrefab();
        }
    }

}

