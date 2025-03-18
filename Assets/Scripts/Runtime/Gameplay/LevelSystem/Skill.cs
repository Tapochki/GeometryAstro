using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public abstract class Skill<T> where T : SkillData
    {
        public int SkillLevel { get; private set; }
        public T SkillData { get; private set; }

        protected IEvent _activateSkillEvent;

        public Skill(T skillData)
        {
            SkillData = skillData;
        }

        public virtual void ApplySkillEffect()
        {
            UpgradeLevel();
        }

        public SkillUpgradeInfo GetUpgradeData()
        {
            SkillUpgradeInfo skillDescription = SkillData.UpgradesInfo[SkillLevel];
            return skillDescription;
        }

        public SkillType GetSkillType()
        {
            return SkillData.Type;
        }

        public void UpgradeLevel()
        {
            SkillLevel++;
        }

        public bool IsMaxLevel()
        {
            return SkillLevel <= SkillData.MaxLevel;
        }
    }

    public class ActiveSkill : Skill<ActiveSkillData>
    {
        public ActiveSkill(ActiveSkillData skillData) : base(skillData) { }

        public bool CanEvolve()
        {
            return SkillData.Evolution != null && IsMaxLevel();
        }

        public void Evolution() 
        {

        }
    }

    public class PassiveSkill : Skill<PassiveSkillData>
    {
        public PassiveSkill(PassiveSkillData skillData) : base(skillData) { }
    }
}

