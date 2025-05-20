using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public abstract class ExplosionEffectContainer<TEvent> : BaseEffectContainer<TEvent, ExplosionEffect>
    where TEvent : struct, IEvent
    {
        protected ExplosionEffectContainer(EffectsConfig effectsConfig) : base(effectsConfig) { }

        protected virtual float CalculateSize(float radius)
        {
            return Mathf.Lerp(1f, 5f, (radius - 10f) / (20f));
        }

        protected void ApplyEffect(ExplosionEffect effect, float radius, Vector3 position)
        {
            float calculatedSizeOfParticleSystem = CalculateSize(radius);
            effect.transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;

            for (int i = 0; i < effect.transform.childCount; i++)
            {
                effect.transform.GetChild(i).transform.localScale = Vector3.one * calculatedSizeOfParticleSystem;
            }

            effect.StartEffect(position);
        }
    }
}