using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class FreezeEffectContainer : IEffectContainer, IEventReceiver<CreateFreezeEffect>
    {
        private readonly FreezeEffectConfig _config;

        private ObjectPool<FreezeEffect> _freezePool;

        private Transform _freezeContainer;

        public UniqueId Id { get; private set; } = new UniqueId();

        public FreezeEffectContainer(EffectsConfig effectsConfig)
        {
            _config = effectsConfig.FreezeEffectConfig;
        }

        public void Init() 
        {
            CreateContainer();

            InitPool();

            RegisterEvent();
        }

        private void CreateContainer() 
        {
            _freezeContainer = new GameObject("[FREEZE_VFX]").transform;
        }

        private void InitPool()
        {
            _freezePool = new ObjectPool<FreezeEffect>(PreloadEffect, OnGetEffect, OnReturnEffect, _config.startPreloadCount);
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CreateFreezeEffect>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CreateFreezeEffect>);
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
            _freezePool.Return((FreezeEffect)effect);
        }

        private FreezeEffect PreloadEffect()
        {
            FreezeEffect effect = Object.Instantiate(_config.effectObject, _freezeContainer).GetComponent<FreezeEffect>();

            effect.Init(ReturnEffect);

            return effect;
        }

        private float CalculateSize(float radius) 
        {
            return Mathf.Lerp(1f, 4f, (radius - 10f) / (20f));
        }
            

        public void OnEvent(CreateFreezeEffect @event)
        {
            FreezeEffect effect = _freezePool.Get();

            float calculatedSizeOfParticleSystem = CalculateSize(@event.Radius);

            effect.transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;

            effect.StartEffect(@event.Position);
        }
    }
}