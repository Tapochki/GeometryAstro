namespace TandC.GeometryAstro.Gameplay 
{
    public interface IFreezeComponent : ITickable
    {
        public bool IsFreeze { get; }
        public void Freeze(float freezeTimer);
    }
}
