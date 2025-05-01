using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class EnemyDeathVFXContainer : IEffectContainer, IEventReceiver<EnemyDeath>
    {
        private readonly EnemyDeathEffectConfig _config;

        private ObjectPool<EnemyDeathEffect> _enemyDeathEffectPool;

        private Transform _enemyDeathEffectContainer;

        public UniqueId Id { get; private set; } = new UniqueId();

        public EnemyDeathVFXContainer(EffectsConfig effectsConfig)
        {
            _config = effectsConfig.EnemyDeathEffectConfig;
        }

        public void Init() 
        {
            CreateContainer();

            InitPool();

            RegisterEvent();
        }

        private void CreateContainer()
        {
            _enemyDeathEffectContainer = new GameObject("[ENEMY_DEATH_VFX]").transform;
        }

        private void InitPool() 
        {
            _enemyDeathEffectPool = new ObjectPool<EnemyDeathEffect>(PreloadEffect, OnGetEffect, OnReturnEffect, _config.startPreloadCount);
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<EnemyDeath>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<EnemyDeath>);
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
            _enemyDeathEffectPool.Return((EnemyDeathEffect)effect);
        }

        private EnemyDeathEffect PreloadEffect()
        {
            EnemyDeathEffect effect = Object.Instantiate(_config.effectObject, _enemyDeathEffectContainer).GetComponent<EnemyDeathEffect>();
            effect.Init(ReturnEffect);
            return effect;
        }

        public void OnEvent(EnemyDeath @event)
        {
            EnemyDeathEffect effect = _enemyDeathEffectPool.Get();

            effect.StartEffect(@event.Position);
        }
    }
}