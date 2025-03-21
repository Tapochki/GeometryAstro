using TandC.GeometryAstro.Data;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IWeaponBuilder
    {
        IWeaponBuilder SetData();
        IWeaponBuilder SetConfig(WeaponConfig config);
        IWeaponBuilder SetProjectileFactory(IReadableModificator damageModificator);
        IWeaponBuilder SetReloader(IReadableModificator reloadModificator);
        IWeaponBuilder SetEnemyDetector();
       // IWeaponBuilder RegisterShootingPatterns();
        IWeapon Build();
    }
}

