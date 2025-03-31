using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DashBuilder : ActiveSkillBuilder<DashSkill>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.Dash;

        private readonly Player _player;
        private readonly SkillInputButton _skillButton;

        public DashBuilder(Player player, SkillInputButton skillButton)
        {
            _player = player;
            _skillButton = skillButton;
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            IReloadable dashReload = new SkillReloader(_activeSkillData.shootDeley, reloadModificator);

            Modificator dashActiveTimeModificator = new Modificator(1, 0, true);

            IReloadable dashTimer = new SkillReloader(_config.AdditionalSkillConfig.DashConfig.DashTime, dashActiveTimeModificator);

            _skill.SetReloader(dashReload, dashTimer, dashActiveTimeModificator, _skillButton);
        }

        private void RegisterPlayerDash()
        {
            Modificator dashActiveTimeModificator = new Modificator(_config.AdditionalSkillConfig.DashConfig.StartDashMultiplier, 0, true);
            IDashMove dashMove = _player.RegisterDashSkill(dashActiveTimeModificator);
            _skill.SetDashComponent(dashMove, dashActiveTimeModificator);
        }

        private void SetSkillPrefab()
        {
            DashView dashView = _skill.InitDashObject(_player.SkillTransform);
            dashView.Init(
                _activeSkillData.bulletData,
                _config.AdditionalSkillConfig.DashConfig.FireTraceSpawnTimer,
                _config.AdditionalSkillConfig.DashConfig.EvolvedColor,
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize)
                );
        }

        protected override void ConstructSkill()
        {
            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));
            RegisterPlayerDash();

            SetSkillPrefab();
        }
    }

}

