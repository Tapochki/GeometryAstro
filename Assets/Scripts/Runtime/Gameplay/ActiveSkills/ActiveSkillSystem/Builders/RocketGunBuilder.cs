using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class RocketGunBuilder : ActiveSkillBuilder<RocketGun>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.RocketGun;

        private int _startBulletPreloadCount = 2;

        private readonly RocketInputButton _rocketButton;

        private Transform _playerTransformSkills;

        public RocketGunBuilder(Transform playerTransformSkills, RocketInputButton rocketButton)
        {
            _playerTransformSkills = playerTransformSkills;
            _rocketButton = rocketButton;
        }

        private void SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator)
        {
            _skill.SetProjectileFactory(damageModificator, criticalChanceModificator, criticalDamageMultiplierModificator, bulletSizeModificator, _startBulletPreloadCount);
        }

        private void SetDuplicatorComponent(IReadableModificator duplicatorModificator)
        {
            _skill.RegisterDuplicatorComponent(duplicatorModificator);
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _skill.SetReloader(new WeaponReloader(_activeSkillData.shootDeley, reloadModificator), _rocketButton);
        }

        private void SetSkillPrefab()
        {
            _skill.RegisterShootingPatterns(_playerTransformSkills);
        }

        private void SetAreaDamageInterval() 
        {
            _skill.SetAreaDamageInterval(_config.AdditionalSkillConfig.AreaEffectConfig.damageInterval);
        }

        protected override void ConstructWeapon()
        {
            SetAreaDamageInterval();

            SetProjectileFactory(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize));

            _skill.InitRocketAmmo();

            SetSkillPrefab();

            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetDuplicatorComponent(_modificatorContainer.GetModificator(ModificatorType.Duplicator));
        }
    }
}

