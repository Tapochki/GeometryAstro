
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class CircleEnemyDetector : IEnemyDetector
    {
        private readonly LayerMask _enemyLayer;

        public CircleEnemyDetector(LayerMask enemyLayer)
        {
            _enemyLayer = enemyLayer;
        }

        protected Collider2D[] TakeCircleHits(Vector2 origin, float maxDistance = 100) 
        {
            return Physics2D.OverlapCircleAll(origin, maxDistance, _enemyLayer);
        }

        protected void DrawCircle(Vector2 origin, float radius)
        {
            Color color = Color.red;
            int segments = 30;
            float angleStep = 360f / segments;
            Vector2 previousPoint = origin + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 newPoint = origin + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);

                Debug.DrawLine(previousPoint, newPoint, color);
                previousPoint = newPoint;
            }
        }

        public abstract Vector2? GetEnemyPosition(Vector2 origin, Vector2 direction = default, float maxDistance = 100);

    }
}

