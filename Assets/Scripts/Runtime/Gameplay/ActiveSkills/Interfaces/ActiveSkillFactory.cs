using TandC.GeometryAstro.Settings;
using UnityEngine;
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
                ActiveSkillType.AutoGun => new AutomaticGunBuilder(),
                ActiveSkillType.StandartGun => new StandardGunBuilder(),
                ActiveSkillType.RocketGun => new RocketGunBuilder(_inputHandler.RocketButton),
                _ => throw new System.ArgumentException("Unknown weapon type")
            };
        }
    }
}
