
using TandC.GeometryAstro.Settings;
using VContainer;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ActiveSkillFactory : IActiveSkillFactory
    {
        private Player _player;
        private IGameplayInputHandler _inputHandler;
        private IActiveSkillController _activeSkillController;
        private IItemSpawner _itemSpawner;

        [Inject]
        private void Construct(Player player, IItemSpawner itemSpawner, IGameplayInputHandler inputHandler) 
        {
            _player = player;
            _inputHandler = inputHandler;
            _itemSpawner = itemSpawner;
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
                ActiveSkillType.RocketGun => new RocketGunBuilder(_player, _itemSpawner, _inputHandler.RocketButton),
                ActiveSkillType.Shield => new ShieldBuilder(_player),
                ActiveSkillType.Cloaking => new CloakBuilder(_player, _inputHandler.CloakButton, _activeSkillController),
                ActiveSkillType.Dash => new DashBuilder(_player, _inputHandler.DashButton),
                ActiveSkillType.LaserDestroyerGun => new LaserBuilder(_player.SkillTransform, _inputHandler.SetInteractable, _inputHandler.LaserButton),
                ActiveSkillType.MachineGun => new MachineGunBuilder(_player.SkillTransform),
                ActiveSkillType.RifleGun => new RifleGunBuilder(_player.SkillTransform),
                _ => throw new System.ArgumentException("Unknown skill type")
            };
        }
    }
}
