using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TandC.GeometryAstro.Gameplay.VFX;
using TandC.GeometryAstro.Services;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class VFXService : IVFXService
    {
        [Inject] private LoadObjectsService _loadObjectsService;

        private List<IEffectContainer> _effects;

        public async UniTask Load()
        {
            Initialize();
            await UniTask.CompletedTask;
        }

        public void RegisterEffectContainers(List<IEffectContainer> effects) 
        {
            _effects = effects;
        }

        private void Initialize()
        {
            foreach (var effect in _effects)
            {
                effect.Init();
            }
        }

        public void Dispose()
        {
            foreach (var effect in _effects)
            {
                effect.Dispose();
            }
        }
    }
}