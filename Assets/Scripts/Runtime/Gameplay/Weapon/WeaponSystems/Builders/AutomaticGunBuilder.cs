using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class AutomaticGunBuilder : WeaponBuilder<AutomaticGun>
    {
        private WeaponType _weaponType = WeaponType.AutoGun;
        private int _startBulletPreloadCount = 5;

        public override IWeaponBuilder SetData()
        {
            _weapon.SetData(_config.GetWeaponByType(_weaponType));
            return this;
        }

        public override IWeaponBuilder SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator)
        {
            _weapon.SetProjectileFactory(new ProjectileFactory(_config.GetWeaponByType(_weaponType).bulletData, _startBulletPreloadCount, damageModificator, criticalChanceModificator, criticalDamageMultiplierModificator, bulletSizeModificator));
            return this;
        }

        public override IWeaponBuilder SetDuplicatorComponent(IReadableModificator duplicatorModificator)
        {
            _weapon.RegisterDuplicatorComponent(duplicatorModificator);
            return this;
        }

        public override IWeaponBuilder SetReloader(IReadableModificator reloadModificator)
        {
            _weapon.SetReloader(new WeaponReloader(_config.GetWeaponByType(_weaponType).shootDeley, reloadModificator));
            return this;
        }

        public override IWeaponBuilder  SetEnemyDetector()
        {
            _weapon.SetEnemyDetector(new CircleEnemyDetector(LayerMask.GetMask("Enemy")));
            return this;
        }
    }
}

