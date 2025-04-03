using TandC.GeometryAstro.Services;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public interface IEffectPool
    {
        void Init(LoadObjectsService loadObjectsService);
        void Dispose();
    }
}