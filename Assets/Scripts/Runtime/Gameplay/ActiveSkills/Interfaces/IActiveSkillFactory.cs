using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IActiveSkillFactory
    {
        public IActiveSkillBuilder GetBuilder(ActiveSkillType type);
        public void SetActiveSkillContainer(IActiveSkillController activeSkillController);
    }
}