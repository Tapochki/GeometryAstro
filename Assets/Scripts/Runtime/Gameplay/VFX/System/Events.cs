using UnityEngine;

namespace TandC.GeometryAstro.EventBus 
{
    public readonly struct CreateExplosionEffect : IEvent
    {
        public readonly Vector3 Position;
        public readonly float Radius;

        public CreateExplosionEffect(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
    }

    public readonly struct CreateFreezeEffect : IEvent
    {
        public readonly Vector3 Position;
        public readonly float Radius;

        public CreateFreezeEffect(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
    }

    public readonly struct CreateDamageAreaEffect : IEvent
    {
        public readonly Vector3 Position;
        public readonly float Radius;
        public readonly float Time;

        public CreateDamageAreaEffect(Vector3 position, float radius, float time)
        {
            Position = position;
            Radius = radius;
            Time = time;
        }
    }

    public readonly struct CreateEnemyDeathEffect : IEvent 
    {
        public readonly Vector3 Position;

        public CreateEnemyDeathEffect(Vector3 position)
        {
            Position = position;
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

