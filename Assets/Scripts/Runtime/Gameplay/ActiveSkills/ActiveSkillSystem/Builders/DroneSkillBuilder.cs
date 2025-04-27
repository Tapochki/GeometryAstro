using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DroneSkillBuilder : ActiveSkillBuilder<DroneSkill>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.Drone;

        private Transform _playerTransform;

        public DroneSkillBuilder(Transform playerTransform) 
        {
            _playerTransform = playerTransform;
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _skill.SetReloader(new SkillReloader(_activeSkillData.shootDeley, reloadModificator));
        }

        private void SetSkillPrefab()
        {
            _skill.SetObject(_playerTransform);
        }

        private void SetSkillView() 
        {
            _skill.SetView(_playerTransform,
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier)
                );
        }

        private void SetSizeModificator() 
        {
            _skill.SetModificators(_modificatorContainer.GetModificator(ModificatorType.BulletsSize), _modificatorContainer.GetModificator(ModificatorType.Duplicator));
        }

        protected override void ConstructSkill()
        {
            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetSkillPrefab();

            SetSkillView();

            SetSizeModificator();
        }
    }
}

