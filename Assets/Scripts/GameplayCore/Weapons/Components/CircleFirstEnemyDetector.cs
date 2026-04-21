using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Returns the first IDamageable enemy found inside a circle radius.
    /// Used by AutomaticGun.
    /// </summary>
    public class CircleFirstEnemyDetector : CircleEnemyDetector
    {
        public CircleFirstEnemyDetector(LayerMask enemyLayer) : base(enemyLayer) { }

        public override Transform GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100f)
        {
            Collider2D[] hits = TakeCircleHits(origin, maxDistance);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable _))
                    return hit.transform;
            }
            return null;
        }
    }
}
