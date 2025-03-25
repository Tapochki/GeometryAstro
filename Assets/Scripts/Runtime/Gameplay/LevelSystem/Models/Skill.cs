using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public abstract class Skill<T> where T : SkillUpgradeData
    {
        public int SkillLevel { get; private set; }
        public T SkillData { get; private set; }

        protected IEvent _activateSkillEvent;

        public Skill(T skillData)
        {
            SkillLevel = 0;
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

        private void UpgradeLevel()
        {
            SkillLevel++;
        }

        public bool IsMaxLevel()
        {
            return SkillLevel >= SkillData.MaxLevel;
        }
    }
}

