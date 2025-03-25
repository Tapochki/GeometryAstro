using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class RocketGunBuilder : ActiveSkillBuilder<RocketGun>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.RocketGun;

        private int _startBulletPreloadCount = 2;

        private void SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator)
        {
            _weapon.SetProjectileFactory(new ProjectileFactory(
                _activeSkillData.bulletData, 
                _startBulletPreloadCount,
                () => Object.Instantiate(_activeSkillData.bulletData.BulletObject).GetComponent<ExplosiveBullet>(),
                damageModificator, 
                criticalChanceModificator, 
                criticalDamageMultiplierModificator, 
                bulletSizeModificator));
        }

        private void SetDuplicatorComponent(IReadableModificator duplicatorModificator)
        {
            _weapon.RegisterDuplicatorComponent(duplicatorModificator);
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _weapon.SetReloader(new WeaponReloader(_activeSkillData.shootDeley, reloadModificator));
        }

        protected override void ConstructWeapon()
        {
            SetProjectileFactory(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize));

            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetDuplicatorComponent(_modificatorContainer.GetModificator(ModificatorType.Duplicator));
        }
    }
}

