using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ShieldKnockbackEffect
    {
        private readonly float _radius;
        private readonly float _force;

        private readonly LayerMask _enemyLayer;

        public ShieldKnockbackEffect(float radius, float force, LayerMask layerMask)
        {
            _radius = radius;
            _force = force;
            _enemyLayer = layerMask;
        }

        public void TriggerEffect(Vector3 position)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, _radius, _enemyLayer);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 knockbackDirection = (enemy.transform.position - position).normalized;
                        rb.AddForce(knockbackDirection * _force, ForceMode2D.Force);
                    }
                }
            }
        }
    }
}

