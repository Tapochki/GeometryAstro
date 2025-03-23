using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IWeapon
    {
        public WeaponType WeaponType { get; } 
        public void SetProjectileFactory(IProjectileFactory projectileFactory);
        public void SetReloader(IReloadable reloader);
        public void SetEnemyDetector(IEnemyDetector enemyDetector);
        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator);
        public void Initialization();
        public void Upgrade();
        public void Tick();
    }
}
