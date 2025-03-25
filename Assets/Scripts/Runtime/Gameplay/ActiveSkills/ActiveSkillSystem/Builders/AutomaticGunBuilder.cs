using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class AutomaticGunBuilder : ActiveSkillBuilder<AutomaticGun>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.AutoGun;

        private int _startBulletPreloadCount = 5;

        private void SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator)
        {
            _weapon.SetProjectileFactory(new ProjectileFactory(
                _activeSkillData.bulletData, 
                _startBulletPreloadCount,
                () => Object.Instantiate(_activeSkillData.bulletData.BulletObject).GetComponent<BulletWithHealth>(),
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

        private void SetEnemyDetector()
        {
            _weapon.SetEnemyDetector(new CircleEnemyDetector(LayerMask.GetMask("Enemy")));
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

            SetEnemyDetector();
        }
    }
}

