using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class DamageAreaEffectContainer : IEffectContainer, IEventReceiver<CreateDamageAreaEffect>
    {
        private readonly DamageAreaEffectConfig _config;

        private ObjectPool<DamageAreaEffect> _explosionParticlesPool;

        private Transform _explosionParticleContainer;

        public UniqueId Id { get; private set; } = new UniqueId();

        public DamageAreaEffectContainer(EffectsConfig effectsConfig)
        {
            _config = effectsConfig.DamageAreaEffectConfig;
        }

        public void Init() 
        {
            CreateContainer();

            InitPool();

            RegisterEvent();
        }

        private void CreateContainer() 
        {
            _explosionParticleContainer = new GameObject("[DAMAGE_AREA_VFX]").transform;
        }

        private void InitPool()
        {
            _explosionParticlesPool = new ObjectPool<DamageAreaEffect>(PreloadEffect, OnGetEffect, OnReturnEffect, _config.startPreloadCount);
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CreateDamageAreaEffect>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CreateDamageAreaEffect>);
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
            _explosionParticlesPool.Return((DamageAreaEffect)effect);
        }

        private DamageAreaEffect PreloadEffect()
        {
            DamageAreaEffect effect = Object.Instantiate(_config.effectObject, _explosionParticleContainer).GetComponent<DamageAreaEffect>();

            effect.Init(ReturnEffect);

            return effect;
        }

        private float CalculateSize(float radius) 
        {
            return Mathf.Lerp(1f, 3f, (radius - 10f) / (20f));
        }
            

        public void OnEvent(CreateDamageAreaEffect @event)
        {
            DamageAreaEffect effect = _explosionParticlesPool.Get();

            float calculatedSizeOfParticleSystem = CalculateSize(@event.Radius);

            effect.transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;

            for (int i = 0; i < effect.transform.childCount; i++)
            {
                effect.transform.GetChild(i).transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;
            }

            effect.StartEffect(@event.Position, @event.Time);
        }
    }
}