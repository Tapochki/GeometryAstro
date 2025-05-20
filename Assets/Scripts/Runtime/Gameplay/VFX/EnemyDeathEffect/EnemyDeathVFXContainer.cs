using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class EnemyDeathVFXContainer : BaseEffectContainer<CreateEnemyDeathEffect, EnemyDeathEffect>
    {
        public EnemyDeathVFXContainer(EffectsConfig effectsConfig) : base(effectsConfig) { }

        protected override BaseEffectConfig GetConfig(EffectsConfig effectsConfig)
        {
            return effectsConfig.EnemyDeathEffectConfig;
        }

        protected override string GetContainerName()
        {
            return "ENEMY_DEATH_VFX";
        }

        public override void OnEvent(CreateEnemyDeathEffect @event)
        {
            EnemyDeathEffect effect = _effectPool.Get();
            effect.StartEffect(@event.Position);
        }
    }
}