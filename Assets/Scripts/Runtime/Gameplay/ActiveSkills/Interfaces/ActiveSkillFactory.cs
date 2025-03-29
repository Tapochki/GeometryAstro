using TandC.GeometryAstro.Settings;
using VContainer;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ActiveSkillFactory : IActiveSkillFactory
    {
        private Player _player;
        private IGameplayInputHandler _inputHandler;

        [Inject]
        private void Construct(Player player, IGameplayInputHandler inputHandler) 
        {
            _player = player;
            _inputHandler = inputHandler;
        }

        public IActiveSkillBuilder GetBuilder(ActiveSkillType type)
        {
            return type switch
            {
                ActiveSkillType.AutoGun => new AutomaticGunBuilder(_player.SkillTransform),
                ActiveSkillType.StandartGun => new StandardGunBuilder(_player.SkillTransform),
                ActiveSkillType.RocketGun => new RocketGunBuilder(_player.SkillTransform, _inputHandler.RocketButton),
                ActiveSkillType.Shield => new ShieldBuilder(_player),
                _ => throw new System.ArgumentException("Unknown weapon type")
            };
        }
    }
}
