using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class DamageAreaEffectContainer : BaseEffectContainer<CreateDamageAreaEffect, DamageAreaEffect>
    {
        public DamageAreaEffectContainer(EffectsConfig effectsConfig) : base(effectsConfig) { }

        protected override BaseEffectConfig GetConfig(EffectsConfig effectsConfig)
        {
            return effectsConfig.DamageAreaEffectConfig;
        }

        protected override string GetContainerName()
        {
            return "DAMAGE_AREA_VFX";
        }

        private float CalculateSize(float radius)
        {
            return Mathf.Lerp(1f, 3f, (radius - 10f) / (20f));
        }

        public override void OnEvent(CreateDamageAreaEffect @event)
        {
            DamageAreaEffect effect = _effectPool.Get();
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