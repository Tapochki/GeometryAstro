using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX 
{
    public abstract class BaseEffectContainer<TEvent, TEffect> : IEffectContainer, IEventReceiver<TEvent>
    where TEvent : struct, IEvent
    where TEffect : MonoBehaviour, IEffect
    {
        protected readonly BaseEffectConfig _baseConfig;
        protected ObjectPool<TEffect> _effectPool;
        protected Transform _effectContainer;

        public UniqueId Id { get; private set; } = new UniqueId();

        protected BaseEffectContainer(EffectsConfig effectsConfig)
        {
            _baseConfig = GetConfig(effectsConfig);
        }

        public void Init()
        {
            CreateContainer();
            InitPool();
            RegisterEvent();
        }

        public void Dispose()
        {
            UnregisterEvent();
        }

        protected virtual void CreateContainer()
        {
            _effectContainer = new GameObject($"[{GetContainerName()}]").transform;
        }

        protected virtual void InitPool()
        {
            _effectPool = new ObjectPool<TEffect>(PreloadEffect, OnGetEffect, OnReturnEffect, _baseConfig.startPreloadCount);
        }

        protected virtual void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<TEvent>);
        }

        protected virtual void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<TEvent>);
        }

        protected virtual void OnReturnEffect(IEffect effect)
        {
            effect.Hide();
        }

        protected virtual void OnGetEffect(IEffect effect)
        {
            effect.Show();
        }

        protected void ReturnEffect(IEffect effect)
        {
            _effectPool.Return((TEffect)effect);
        }

        protected virtual TEffect PreloadEffect()
        {
            TEffect effect = Object.Instantiate(_baseConfig.effectObject, _effectContainer).GetComponent<TEffect>();
            effect.Init(ReturnEffect);
            return effect;
        }

        public abstract void OnEvent(TEvent @event);

        protected abstract BaseEffectConfig GetConfig(EffectsConfig effectsConfig);

        protected abstract string GetContainerName();

    }
}

