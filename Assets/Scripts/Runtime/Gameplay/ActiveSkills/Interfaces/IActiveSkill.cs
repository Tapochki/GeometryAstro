using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{

    public interface IActiveSkill
    {
        public ActiveSkillType SkillType { get; }

        public void SetData(ActiveSkillData data);

        public void Initialization();
        public void Upgrade();
        public void Tick();
    }
}
