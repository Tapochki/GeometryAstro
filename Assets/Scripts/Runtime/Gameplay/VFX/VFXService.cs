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

            var calculatedSizeOfParticleSystem = 0.0f;
            switch (@event.Radius)
            {
                case 10:
                    calculatedSizeOfParticleSystem = 1;
                    break;
                case 15:
                    calculatedSizeOfParticleSystem = 1.2f;
                    break;
                case 20:
                    calculatedSizeOfParticleSystem = 1.4f;
                    break;
                case 25:
                    calculatedSizeOfParticleSystem = 1.6f;
                    break;
                case 30:
                    calculatedSizeOfParticleSystem = 1.8f;
                    break;
                default:
                    Debug.LogWarning($"Current radius is {@event.Radius} not supported");
                    break;
            }
            item.transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;
            item.Init(ReturnExplosionParticle);
        }
    }
}