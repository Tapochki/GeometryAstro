using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CloakBuilder : ActiveSkillBuilder<CloakingSkill>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.Shield;

        private readonly Player _player;
        private readonly SkillInputButton _skillButton;
        private readonly IActiveSkillController _activeSkillController;

        public CloakBuilder(Player player, SkillInputButton skillButton, IActiveSkillController activeSkillController)
        {
            _player = player;
            _skillButton = skillButton;
            _activeSkillController = activeSkillController;
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            IReloadable cloakReload = new SkillReloader(_activeSkillData.shootDeley, reloadModificator);

            Modificator cloakActiveTimeModificator = new Modificator(1, 0, true);

            IReloadable activeCloakTimer = new SkillReloader(_config.AdditionalSkillConfig.CloakConfig.StartCloakActiveTime, cloakActiveTimeModificator);

            _skill.SetReloader(cloakReload, activeCloakTimer, cloakActiveTimeModificator, _skillButton);
        }

        private void RegisterCloakReciavers()
        {
            _activeSkillController.RegisterMask();
            _player.RegisterCloak();
        }

        protected override void ConstructWeapon()
        {
            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            RegisterCloakReciavers();
        }
    }

}

