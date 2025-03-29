namespace TandC.GeometryAstro.Gameplay
{
    public interface IActiveSkillController
    {
        public void Init();
        public void Dispose();

        public void RegisterMask();
    }
}