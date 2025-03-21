using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Settings;
using UnityEngine;

public class StandardGunBuilder : WeaponBuilder<StandardGun>
{
    private WeaponType _weaponType = WeaponType.StandardGun;
    private int _startBulletPreloadCount = 10;

    public override IWeaponBuilder SetData()
    {
        _weapon.SetData(_config.GetWeaponByType(_weaponType));
        return this;
    }

    public override IWeaponBuilder SetProjectileFactory(IReadableModificator damageModificator)
    {
        _weapon.SetProjectileFactory(new ProjectileFactory(_config.GetWeaponByType(_weaponType).bulletData, _startBulletPreloadCount, damageModificator));
        return this;
    }

    public override IWeaponBuilder SetReloader(IReadableModificator reloadModificator)
    {
        _weapon.SetReloader(new WeaponReloader(_config.GetWeaponByType(_weaponType).shootDeley, reloadModificator));
        return this;
    }

    public override IWeaponBuilder SetEnemyDetector()
    {
        _weapon.SetEnemyDetector(new RaycastEnemyDetector(LayerMask.GetMask("Enemy")));
        return this;
    }
}

