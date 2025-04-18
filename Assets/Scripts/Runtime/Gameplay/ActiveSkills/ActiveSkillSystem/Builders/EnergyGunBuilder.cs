using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class EnergyGunBuilder : ActiveSkillBuilder<EnergyGun>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.EnergyGun;

        private Transform _playerTransformSkills;

        private int _startBulletPreloadCount = 5;

        public EnergyGunBuilder(Transform playerTransformSkills) 
        {
            _playerTransformSkills = playerTransformSkills;
        }

        private void SetReloader()
        {
            _skill.SetReloader(new SkillReloader(_activeSkillData.shootDeley, new Modificator(1, 0, false)));
        }

        private void SetEnemyDetector()
        {
            _skill.SetEnemyDetector(new GroupCircleEnemyDetector(LayerMask.GetMask("Enemy")));
        }

        private void SetSkillPrefab()
        {
            _skill.SetWeaponObject(_playerTransformSkills);
        }

        protected override void ConstructSkill()
        {
            _skill.SetModificators(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetReloader();

            SetSkillPrefab();

            SetEnemyDetector();
        }
    }
}

