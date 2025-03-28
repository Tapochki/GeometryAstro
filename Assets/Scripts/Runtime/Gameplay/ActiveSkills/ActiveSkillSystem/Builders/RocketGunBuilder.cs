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
            _weapon.SetProjectileFactory(damageModificator, criticalChanceModificator, criticalDamageMultiplierModificator, bulletSizeModificator, _startBulletPreloadCount);
        }

        private void SetDuplicatorComponent(IReadableModificator duplicatorModificator)
        {
            _weapon.RegisterDuplicatorComponent(duplicatorModificator);
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _weapon.SetReloader(new WeaponReloader(_activeSkillData.shootDeley, reloadModificator), _rocketButton);
        }

        private void SetSkillPrefab()
        {
            _weapon.RegisterShootingPatterns(_playerTransformSkills);
        }

        protected override void ConstructWeapon()
        {
            SetProjectileFactory(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize));

            _weapon.InitRocketAmmo();

            SetSkillPrefab();

            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetDuplicatorComponent(_modificatorContainer.GetModificator(ModificatorType.Duplicator));
        }
    }
}

