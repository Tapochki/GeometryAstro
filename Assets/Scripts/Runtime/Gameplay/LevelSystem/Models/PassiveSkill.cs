using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public class PassiveSkill : Skill<PassiveSkillData>
    {
        private readonly ModificatorType _upgradablePassiveSkillType;

        public PassiveSkill(PassiveSkillData skillData, ModificatorType upgradablePassiveSkillType) : base(skillData) 
        {
            _upgradablePassiveSkillType = upgradablePassiveSkillType;
            ApplySkillEffect();
        }

        public override void ApplySkillEffect()
        {
            EventBusHolder.EventBus.Raise(new PassiveSkillUpgradeEvent(_upgradablePassiveSkillType));
            base.ApplySkillEffect();
        }
    }
}

