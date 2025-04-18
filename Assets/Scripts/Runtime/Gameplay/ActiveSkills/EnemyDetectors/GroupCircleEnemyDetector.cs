
using System.Collections.Generic;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class GroupCircleEnemyDetector : IEnemyDetector
    {
        private readonly LayerMask _enemyLayer;

        public GroupCircleEnemyDetector(LayerMask enemyLayer)
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

        protected List<Enemy> FilterEnemies(Collider2D[] hits)
        {
            List<Enemy> enemies = new List<Enemy>();
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    enemies.Add(enemy);
                }
            }
            return enemies;
        }

        public List<Enemy> GetEnemyGroup(Vector2 origin, float maxDistance = 100) 
        {
            Collider2D[] hits = TakeCircleHits(origin, maxDistance);
            return FilterEnemies(hits);
        }

        public Enemy GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100)
        {
            return null;
        }
    }
}

