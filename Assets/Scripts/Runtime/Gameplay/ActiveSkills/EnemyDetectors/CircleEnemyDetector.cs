
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CircleEnemyDetector : IEnemyDetector
    {
        private readonly LayerMask _enemyLayer;

        public CircleEnemyDetector(LayerMask enemyLayer)
        {
            _enemyLayer = enemyLayer;
        }

        public Vector2? GetEnemyPosition(Vector2 origin, Vector2 direction = default, float maxDistance = 100)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(origin, maxDistance, _enemyLayer);
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
            if(nearestEnemy == null) 
            {
                return null;
            }

            return nearestEnemy.transform.position;
        }
    }
}

