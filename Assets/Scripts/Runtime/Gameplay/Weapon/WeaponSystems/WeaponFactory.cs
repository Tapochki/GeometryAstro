using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public class WeaponFactory : IWeaponFactory
    {
        public WeaponFactory() 
        {

        }

        public IWeaponBuilder GetBuilder(WeaponType type)
        {
            return type switch
            {
                WeaponType.AutoGun => new AutomaticGunBuilder(),
                WeaponType.StandardGun => new StandardGunBuilder(),
                WeaponType.RocketGun => new RocketGunBuilder(),
                _ => throw new System.ArgumentException("Unknown weapon type")
            };
        }
    }
}
