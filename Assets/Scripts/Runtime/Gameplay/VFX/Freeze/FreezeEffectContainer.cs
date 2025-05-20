using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class FreezeEffectContainer : ExplosionEffectContainer<CreateFreezeEffect>
    {
        public FreezeEffectContainer(EffectsConfig effectsConfig) : base(effectsConfig) { }

        protected override BaseEffectConfig GetConfig(EffectsConfig effectsConfig)
        {
            return effectsConfig.FreezeEffectConfig;
        }

        protected override string GetContainerName()
        {
            return "FREEZE_VFX";
        }

        public override void OnEvent(CreateFreezeEffect @event)
        {
            ExplosionEffect effect = _effectPool.Get();
            ApplyEffect(effect, @event.Radius, @event.Position);
        }
    }
}