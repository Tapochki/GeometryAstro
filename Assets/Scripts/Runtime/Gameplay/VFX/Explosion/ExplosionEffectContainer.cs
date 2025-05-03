using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class ExplosionEffectContainer : IEffectContainer, IEventReceiver<CreateExplosionEffect>
    {
        private readonly ExplosionEffectConfig _config;

        private ObjectPool<ExplosionEffect> _explosionParticlesPool;

        private Transform _explosionParticleContainer;

        public UniqueId Id { get; private set; } = new UniqueId();

        public ExplosionEffectContainer(EffectsConfig effectsConfig)
        {
            _config = effectsConfig.ExplosionEffectConfig;
        }

        public void Init() 
        {
            CreateContainer();

            InitPool();

            RegisterEvent();
        }

        private void CreateContainer() 
        {
            _explosionParticleContainer = new GameObject("[EXPLOSION_VFX]").transform;
        }

        private void InitPool()
        {
            _explosionParticlesPool = new ObjectPool<ExplosionEffect>(PreloadEffect, OnGetEffect, OnReturnEffect, _config.startPreloadCount);
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CreateExplosionEffect>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CreateExplosionEffect>);
        }

        public void Dispose()
        {
            UnregisterEvent();
        }

        private void OnReturnEffect(IEffect effect)
        {
            effect.Hide();
        }

        private void OnGetEffect(IEffect effect)
        {
            effect.Show();
        }

        private void ReturnEffect(IEffect effect) 
        {
            _explosionParticlesPool.Return((ExplosionEffect)effect);
        }

        private ExplosionEffect PreloadEffect()
        {
            ExplosionEffect effect = Object.Instantiate(_config.effectObject, _explosionParticleContainer).GetComponent<ExplosionEffect>();

            effect.Init(ReturnEffect);

            return effect;
        }

        private float CalculateSize(float radius) 
        {
            return Mathf.Lerp(1f, 3f, (radius - 10f) / (20f));
        }
            

        public void OnEvent(CreateExplosionEffect @event)
        {
            ExplosionEffect effect = _explosionParticlesPool.Get();

            float calculatedSizeOfParticleSystem = CalculateSize(@event.Radius);

            effect.transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;

            for (int i = 0; i < effect.transform.childCount; i++)
            {
                effect.transform.GetChild(i).transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;
            }

            effect.StartEffect(@event.Position);
        }
    }
}