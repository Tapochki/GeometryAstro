using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class ActiveSkillBuilder<T> : IActiveSkillBuilder where T : IActiveSkill, new()
    {
        protected T _skill;
        protected ModificatorContainer _modificatorContainer;

        protected abstract ActiveSkillType _activeSkillType { get; }

        protected ActiveSkillData _activeSkillData;

        protected ActiveSkillConfig _config;

        public ActiveSkillBuilder()
        {
            _skill = new T();
        }

        public IActiveSkillBuilder SetConfig(ActiveSkillConfig config)
        {
            _config = config;
            _activeSkillData = _config.GetActiveSkillByType(_activeSkillType);
            _skill.SetData(_activeSkillData);
            return this;
        }

        public IActiveSkillBuilder SetModificators(ModificatorContainer modificatorContainer)
        {
            _modificatorContainer = modificatorContainer;
            return this;
        }

        protected abstract void ConstructSkill();

        public IActiveSkill Build()
        {
            ConstructSkill();
            return _skill;
        }
    }
}

