using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class ActiveSkillBuilder<T> : IActiveSkillBuilder where T : IActiveSkill, new()
    {
        protected T _weapon;
        protected ModificatorContainer _modificatorContainer;

        protected abstract ActiveSkillType _activeSkillType { get; }

        protected ActiveSkillData _activeSkillData;

        public ActiveSkillBuilder()
        {
            _weapon = new T();
        }

        public IActiveSkillBuilder SetConfig(ActiveSkillConfig config)
        {
            _activeSkillData = config.GetActiveSkillByType(_activeSkillType);
            _weapon.SetData(_activeSkillData);
            return this;
        }

        public IActiveSkillBuilder SetModificators(ModificatorContainer modificatorContainer)
        {
            _modificatorContainer = modificatorContainer;
            return this;
        }

        protected abstract void ConstructWeapon();

        public IActiveSkill Build()
        {
            ConstructWeapon();
            return _weapon;
        }
    }
}

