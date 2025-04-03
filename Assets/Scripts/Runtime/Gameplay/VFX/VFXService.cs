using Cysharp.Threading.Tasks;
using TandC.GeometryAstro.Gameplay.VFX;
using TandC.GeometryAstro.Services;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class VFXService : ILoadUnit
    {
        [Inject] private LoadObjectsService _loadObjectsService;

        private ExplosionEffectPool _explosionEffectPool;

        public async UniTask Load()
        {

            Initialize();
            await UniTask.CompletedTask;
        }

        private void Initialize()
        {
            _explosionEffectPool.Init(_loadObjectsService);
        }

        public void Dispose()
        {
            _explosionEffectPool.Dispose();
        }
    }
}