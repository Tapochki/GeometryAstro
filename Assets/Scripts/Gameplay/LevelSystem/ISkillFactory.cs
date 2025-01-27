using TandC.Data;

namespace TandC.Gameplay
{
    public interface ISkillFactory
    {
        public Skill CreateSkill(SkillData skillData);
    }
}

