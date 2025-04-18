
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CircleNearEnemyDetector : CircleEnemyDetector
    {
        public CircleNearEnemyDetector(LayerMask enemyLayer) : base(enemyLayer) { }

        public override Enemy GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100)
        {
            Collider2D[] hits = TakeCircleHits(origin, maxDistance);

            DrawCircle(origin, maxDistance);

            Enemy nearestEnemy = null;
            float minDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    float distance = Vector2.Distance(origin, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }
            if (nearestEnemy == null)
            {
                return null;
            }

            return nearestEnemy;
        }
    }
}

