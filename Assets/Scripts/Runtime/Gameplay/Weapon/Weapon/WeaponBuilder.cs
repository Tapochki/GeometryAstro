using TandC.GeometryAstro.Data;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class WeaponBuilderBase<T> : IWeaponBuilder<T> where T : IWeapon
    {
        protected IReloadable _reloader;
        protected IEnemyDetector _detector;

        public void ConfigureReload(IReloadable reloader) => _reloader = reloader;
        public void ConfigureDetection(IEnemyDetector detector) => _detector = detector;

        public abstract T Build(WeaponData data, IProjectileFactory projectileFactory);
    }

    //public class StandardGunBuilder : WeaponBuilderBase<StandardGun>
    //{
    //    public override StandardGun Build(WeaponData data, IProjectileFactory projectileFactory)
    //    {
    //        var gun = new StandardGun(projectileFactory, data, _reloader, _detector);
    //        return gun;
    //    }
    //}
}

