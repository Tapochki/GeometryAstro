using Cysharp.Threading.Tasks;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class VFXService : ILoadUnit, IEventReceiver<CreateExplosion>
    {
        private ObjectPool<ParticleSystem> _explosionParticlesPool;

        [Inject] private LoadObjectsService _loadObjectsService;

        private ParticleSystem _explosionParticlePrefab;

        public UniqueId Id { get; private set; } = new UniqueId();

        public async UniTask Load()
        {
            EventBusHolder.EventBus.Register(this);

            Initialize();
            await UniTask.CompletedTask;
        }

        private void Initialize()
        {
            _explosionParticlePrefab = _loadObjectsService.GetObjectByPath<ParticleSystem>("Prefabs/Gameplay/VFX/RocketBoomVFX");

            _explosionParticlesPool = new ObjectPool<ParticleSystem>
                (PreloadExplosionParticle, GetExplosionParticle, ReturnExplosionParticle, 10);
        }

        private void ReturnExplosionParticle(ParticleSystem system)
        {
            _explosionParticlesPool.Return(system);
        }

        private void GetExplosionParticle(ParticleSystem system)
        {
            system.gameObject.SetActive(false);
        }

        private ParticleSystem PreloadExplosionParticle()
        {
            ParticleSystem item = MonoBehaviour.Instantiate(_explosionParticlePrefab).GetComponent<ParticleSystem>();
            return item;
        }

        public void Dispose()
        {
            EventBusHolder.EventBus.Unregister(this);
        }

        public void OnEvent(CreateExplosion @event)
        {
            ParticleSystem item = _explosionParticlesPool.Get();
            item.transform.position = @event.Position;
        }
    }
}