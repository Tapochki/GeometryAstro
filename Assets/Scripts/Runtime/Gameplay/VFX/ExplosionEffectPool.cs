using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class ExplosionEffectPool : IEffectPool, IEventReceiver<CreateExplosion>
    {
        private ObjectPool<ExplosionEffect> _explosionParticlesPool;

        private ExplosionEffect _explosionParticlePrefab;

        private Transform _explosionParticleContainer;
        public UniqueId Id { get; private set; } = new UniqueId();

        public void Init(LoadObjectsService loadObjectsService)
        {
            _explosionParticleContainer = new GameObject("[EXPLOSION_VFX]").transform;
            _explosionParticlePrefab = loadObjectsService.GetObjectByPath<ExplosionEffect>("Prefabs/Gameplay/VFX/Explosion_Effect");

            _explosionParticlesPool = new ObjectPool<ExplosionEffect>
                (PreloadExplosionParticle, GetExplosionParticle, ReturnExplosionParticle, 10);

            EventBusHolder.EventBus.Register(this);
        }

        public void Dispose()
        {
            EventBusHolder.EventBus.Unregister(this);
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