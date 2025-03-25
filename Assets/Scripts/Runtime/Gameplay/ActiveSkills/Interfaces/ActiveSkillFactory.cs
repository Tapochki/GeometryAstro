using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ActiveSkillFactory : IActiveSkillFactory
    {
        public ActiveSkillFactory() 
        {

        }

        public IActiveSkillBuilder GetBuilder(ActiveSkillType type)
        {
            return type switch
            {
                ActiveSkillType.AutoGun => new AutomaticGunBuilder(),
                ActiveSkillType.StandartGun => new StandardGunBuilder(),
                ActiveSkillType.RocketGun => new RocketGunBuilder(),
                _ => throw new System.ArgumentException("Unknown weapon type")
            };
        }
    }
}
