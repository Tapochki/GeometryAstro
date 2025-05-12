using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class DamageVFXContainer : IEffectContainer, IEventReceiver<CreateDamageEffect>
    {
        private const int _preloadEffectCount = 50;

        private readonly DamageEffectConfig _config;
        private readonly DataService _dataService;

        private ObjectPool<DamageEffect> _damageParticlePool;

        private Transform _damageEffectContainer;

        public UniqueId Id { get; private set; } = new UniqueId();

        public DamageVFXContainer(EffectsConfig effectsConfig, DataService dataService)
        {
            _config = effectsConfig.DamageEffectConfig;
            _dataService = dataService;
        }

        public void Init() 
        {
            CreateContainer();

            InitPool();

            RegisterEvent();
        }

        private void CreateContainer()
        {
            _damageEffectContainer = new GameObject("[DAMAGE_VFX]").transform;
        }

        private void InitPool() 
        {
            _damageParticlePool = new ObjectPool<DamageEffect>(PreloadEffect, OnGetEffect, OnReturnEffect, _config.startPreloadCount);
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CreateDamageEffect>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CreateDamageEffect>);
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
            _damageParticlePool.Return((DamageEffect)effect);
        }

        private DamageEffect PreloadEffect()
        {
            DamageEffect effect = Object.Instantiate(_config.effectObject, _damageEffectContainer).GetComponent<DamageEffect>();
            effect.Init(ReturnEffect);
            return effect;
        }
        private void StartEffect(Vector3 position, int damage, Color color)
        {
            DamageEffect effect = _damageParticlePool.Get();
            effect.StartEffect(position, damage, color);
        }

        public void OnEvent(CreateDamageEffect @event)
        {
            bool isVisible = @event.IsCrit ? _dataService.AppSettingsData._isShowCritDamageEffect
                                           : _dataService.AppSettingsData._isShowDamageEffect;

            if (!isVisible)
                return;

            Color color = @event.IsCrit ? _config.CritColor : _config.StandartColor;

            StartEffect(@event.Position, (int)@event.Damage, color);
        }
    }
}