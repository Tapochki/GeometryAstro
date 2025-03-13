using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IWeaponBuilder
    {
        IWeaponBuilder SetData();
        IWeaponBuilder SetConfig(WeaponConfig config);
        IWeaponBuilder SetProjectileFactory();
        IWeaponBuilder SetReloader();
        IWeaponBuilder SetEnemyDetector();
       // IWeaponBuilder RegisterShootingPatterns();
        IWeapon Build();
    }
}

