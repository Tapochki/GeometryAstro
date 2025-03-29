
using TandC.GeometryAstro.Settings;
using VContainer;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ActiveSkillFactory : IActiveSkillFactory
    {
        private Player _player;
        private IGameplayInputHandler _inputHandler;
        private IActiveSkillController _activeSkillController;

        [Inject]
        private void Construct(Player player, IGameplayInputHandler inputHandler) 
        {
            _player = player;
            _inputHandler = inputHandler;
        }

        public void SetActiveSkillContainer(IActiveSkillController activeSkillController) 
        {
            _activeSkillController = activeSkillController;
        }

        public IActiveSkillBuilder GetBuilder(ActiveSkillType type)
        {
            return type switch
            {
                ActiveSkillType.AutoGun => new AutomaticGunBuilder(_player.SkillTransform),
                ActiveSkillType.StandartGun => new StandardGunBuilder(_player.SkillTransform),
                ActiveSkillType.RocketGun => new RocketGunBuilder(_player.SkillTransform, _inputHandler.RocketButton),
                ActiveSkillType.Shield => new ShieldBuilder(_player),
                ActiveSkillType.Cloaking => new CloakBuilder(_player, _inputHandler.CloakButton, _activeSkillController),
                _ => throw new System.ArgumentException("Unknown skill type")
            };
        }
    }
}
