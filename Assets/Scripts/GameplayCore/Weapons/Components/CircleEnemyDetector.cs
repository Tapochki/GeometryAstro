using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Abstract base for detectors that use Physics2D.OverlapCircleAll.
    /// </summary>
    public abstract class CircleEnemyDetector : IEnemyDetector
    {
        private readonly LayerMask _enemyLayer;

        protected CircleEnemyDetector(LayerMask enemyLayer)
        {
            _enemyLayer = enemyLayer;
        }

        protected Collider2D[] TakeCircleHits(Vector2 origin, float maxDistance)
            => Physics2D.OverlapCircleAll(origin, maxDistance, _enemyLayer);

        protected void DrawCircle(Vector2 origin, float radius)
        {
            int segments = 30;
            float angleStep = 360f / segments;
            Vector2 prev = origin + new Vector2(radius, 0);
            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 next = origin + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                Debug.DrawLine(prev, next, Color.red);
                prev = next;
            }
        }

        /// <summary>Returns the Transform of the found enemy, or null.</summary>
        public abstract Transform GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100f);
    }
}
