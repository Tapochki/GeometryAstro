using TandC.GeometryAstro.Data;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IWeaponBuilder<T> where T : IWeapon
    {
        T Build(WeaponData data, IProjectileFactory projectileFactory);
        void ConfigureReload(IReloadable reloader);
        void ConfigureDetection(IEnemyDetector detector);
    }
}

