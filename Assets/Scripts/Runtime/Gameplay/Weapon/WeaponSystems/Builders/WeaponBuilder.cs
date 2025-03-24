using TandC.GeometryAstro.Data;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class WeaponBuilder<T> : IWeaponBuilder where T : IWeapon, new()
    {
        protected T _weapon;
        protected WeaponConfig _config;

        public WeaponBuilder()
        {
            _weapon = new T();
        }

        public IWeaponBuilder SetConfig(WeaponConfig config)
        {
            _config = config;
            SetData();
            return this;
        }

        public abstract IWeaponBuilder SetData();
        public abstract IWeaponBuilder SetDuplicatorComponent(IReadableModificator duplicatorModificator);
        public abstract IWeaponBuilder SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator);
        public abstract IWeaponBuilder SetReloader(IReadableModificator reloadModificator);
        public abstract IWeaponBuilder SetEnemyDetector();

        public IWeapon Build()
        {
            return _weapon;
        }
    }
}

