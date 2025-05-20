using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class ExplosionEffectContainer : ExplosionEffectContainer<CreateExplosionEffect>
    {
        public ExplosionEffectContainer(EffectsConfig effectsConfig) : base(effectsConfig) { }

        protected override BaseEffectConfig GetConfig(EffectsConfig effectsConfig)
        {
            return effectsConfig.ExplosionEffectConfig;
        }

        protected override string GetContainerName()
        {
            return "EXPLOSION_VFX";
        }

        public override void OnEvent(CreateExplosionEffect @event)
        {
            ExplosionEffect effect = _effectPool.Get();
            ApplyEffect(effect, @event.Radius, @event.Position);
        }
    }
}