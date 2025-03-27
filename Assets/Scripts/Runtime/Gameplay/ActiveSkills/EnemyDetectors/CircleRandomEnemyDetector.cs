
using System.Collections.Generic;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CircleRandomEnemyDetector : CircleEnemyDetector
    {
        public CircleRandomEnemyDetector(LayerMask enemyLayer): base(enemyLayer) { }

        public override Vector2? GetEnemyPosition(Vector2 origin, Vector2 direction = default, float maxDistance = 100)
        {
            Collider2D[] hits = TakeCircleHits(origin, maxDistance);

            DrawCircle(origin, maxDistance);

            List<Enemy> enemies = new List<Enemy>();

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    enemies.Add(enemy);
                }
            }

            if (enemies.Count == 0)
            {
                return null;
            }

            Enemy randomEnemy = enemies[Random.Range(0, enemies.Count)];

            Debug.LogError(randomEnemy.transform.position);

            return randomEnemy.transform.position;
        }
    }
}

