using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IWeaponFactory
    {
        IWeapon CreateWeapon(WeaponType type);
    }
}

