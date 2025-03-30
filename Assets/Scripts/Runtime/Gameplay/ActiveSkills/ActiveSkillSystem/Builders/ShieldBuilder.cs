using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ShieldBuilder : ActiveSkillBuilder<ShieldSkill>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.Shield;

        private Player _player;

        public ShieldBuilder(Player player)
        {
            _player = player;
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _skill.SetReloader(new SkillReloader(_activeSkillData.shootDeley, reloadModificator));
        }

        private void SetSkillPrefab()
        {
            _skill.InitShieldObject(_player.SkillTransform);
        }

        private void SetColorConfig()
        {
            _skill.SetShieldColorConfig(_config.AdditionalSkillConfig.ShieldColorConfig);
        }

        protected override void ConstructSkill()
        {
            SetColorConfig();

            SetSkillPrefab();

            _skill.SetPlayerShield(_player);

            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));
        }
    }
}


