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
        private ObjectPool<ExplosionEffect> _explosionParticlesPool;

        [Inject] private LoadObjectsService _loadObjectsService;

        private ExplosionEffect _explosionParticlePrefab;

        private Transform _explosionParticleContainer;
        public UniqueId Id { get; private set; } = new UniqueId();

        public async UniTask Load()
        {
            EventBusHolder.EventBus.Register(this);

            Initialize();
            await UniTask.CompletedTask;
        }

        private void Initialize()
        {
            _explosionParticleContainer = new GameObject("[EXPLOSION_VFX]").transform;
            _explosionParticlePrefab = _loadObjectsService.GetObjectByPath<ExplosionEffect>("Prefabs/Gameplay/VFX/Explosion_Effect");

            _explosionParticlesPool = new ObjectPool<ExplosionEffect>
                (PreloadExplosionParticle, GetExplosionParticle, ReturnExplosionParticle, 10);
        }

        private void ReturnExplosionParticle(ExplosionEffect system)
        {
            system.gameObject.SetActive(false);
        }

        private void GetExplosionParticle(ExplosionEffect system)
        {
            system.gameObject.SetActive(true);
        }

        private ExplosionEffect PreloadExplosionParticle()
        {
            ExplosionEffect item = Object.Instantiate(_explosionParticlePrefab, _explosionParticleContainer);
            return item;
        }

        public void Dispose()
        {
            EventBusHolder.EventBus.Unregister(this);
        }

        public void OnEvent(CreateExplosion @event)
        {
            ExplosionEffect item = _explosionParticlesPool.Get();
            item.transform.position = @event.Position;

            float calculatedSizeOfParticleSystem = Mathf.Lerp(1f, 1.8f, (@event.Radius - 10f) / (30f - 10f));

            item.transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;

            for (int i = 0; i < item.transform.childCount; i++)
            {
                item.transform.GetChild(i).transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;
            }

            item.Init(ReturnExplosionParticle);
        }
    }
}