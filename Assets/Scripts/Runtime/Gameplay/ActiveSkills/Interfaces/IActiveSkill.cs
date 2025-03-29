using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{

    public interface IActiveSkill
    {
        public bool IsWeapon { get; }

        public ActiveSkillType SkillType { get; }

        public void SetData(ActiveSkillData data);

        public void Initialization();

        public void Evolve();
        public void Upgrade(float Value = 0);
        public void Tick();
    }
}
