using System.Collections.Generic;
using TandC.GeometryAstro.Gameplay.VFX;
using TandC.GeometryAstro.Services;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IVFXService : ILoadUnit
    {
        void RegisterEffectContainers(List<IEffectContainer> effects);
        void Dispose();
    }
}

