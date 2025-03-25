using TandC.GeometryAstro.Data;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IActiveSkillBuilder
    {
        IActiveSkillBuilder SetConfig(ActiveSkillConfig config);
        IActiveSkillBuilder SetModificators(ModificatorContainer modificatorContainer);
        IActiveSkill Build();
    }
}

