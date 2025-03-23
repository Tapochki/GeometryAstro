using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public class RocketGunBuilder : WeaponBuilder<RocketGun>
    {
        private WeaponType _weaponType = WeaponType.RocketGun;
        private int _startBulletPreloadCount = 2;

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

        public override IWeaponBuilder SetEnemyDetector()
        {
            return this;
        }
    }
}

