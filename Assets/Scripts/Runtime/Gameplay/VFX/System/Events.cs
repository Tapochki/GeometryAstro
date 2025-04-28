using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.EventBus 
{
    public readonly struct CreateExplosion : IEvent
    {
        public readonly Vector3 Position;
        public readonly float Radius;

        public CreateExplosion(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
    }

    public readonly struct CreateDamageEffect : IEvent
    {
        public readonly float Damage;
        public readonly Vector3 Position;
        public readonly bool IsCrit;

        public CreateDamageEffect(float damage, Vector3 position, bool isCrit)
        {
            Damage = damage;
            Position = position;
            IsCrit = isCrit;
        }
    }
}

