using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public class WeaponFactory
    {
        private readonly Dictionary<WeaponType, (IWeaponBuilder<IWeapon> builder, Func<IProjectileFactory> projectileFactory)> _builders = new();
        private readonly WeaponConfig _weaponConfig;

        public WeaponFactory(WeaponConfig weaponConfig)
        {
            _weaponConfig = weaponConfig;
        }

        public void RegisterBuilder<T>(WeaponType type, IWeaponBuilder<T> builder, Func<IProjectileFactory> projectileFactory) where T : IWeapon
        {
           // _builders[type] = (builder, projectileFactory);
        }

        public IWeapon CreateWeapon(WeaponType type)
        {
            if (!_builders.TryGetValue(type, out var entry))
            {
                throw new ArgumentException($"No builder registered for weapon type: {type}");
            }

            return entry.builder.Build(_weaponConfig.GetWeaponByType(type), entry.projectileFactory());
        }
    }
}
