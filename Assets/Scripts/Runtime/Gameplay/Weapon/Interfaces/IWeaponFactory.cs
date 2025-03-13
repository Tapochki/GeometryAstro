using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IWeaponFactory
    {
        public IWeaponBuilder GetBuilder(WeaponType type);
    }
}

