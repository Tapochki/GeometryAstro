using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public class ActiveSkill : Skill<ActiveSkillData>
    {
        private readonly ActiveSkillType _upgradableActiveSkillType;
        private bool _hasAlreadyEvolved;

        public ActiveSkill(ActiveSkillData skillData, ActiveSkillType upgradableActiveSkillType) : base(skillData) 
        {
            _hasAlreadyEvolved = false;
            _upgradableActiveSkillType = upgradableActiveSkillType;
            ApplySkillEffect();
        }

        public bool CanEvolve()
        {
            return SkillData.EvolutionData.TypeForEvolution != SkillType.None && IsMaxLevel() && !_hasAlreadyEvolved;
        }

        public void Evolve() 
        {
            EventBusHolder.EventBus.Raise(new ActiveSkillEvolveEvent(_upgradableActiveSkillType));
            _hasAlreadyEvolved = true;
        }

        public override void ApplySkillEffect()
        {
            EventBusHolder.EventBus.Raise(new ActiveSkillUpgradeEvent(_upgradableActiveSkillType));
            base.ApplySkillEffect();
        }
    }
}

