using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ExplosionDamage
    {
        public void ApplyExplosionDamage(Vector2 origin, float radius, float damage, float critChance, float critMultiplier)
        {
            Debug.LogError(radius);
            Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, LayerMask.GetMask("Enemy"));

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(damage, critChance, critMultiplier);
                }
            }
        }
    }
}

