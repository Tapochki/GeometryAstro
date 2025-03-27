
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CircleFirstEnemyDetector : CircleEnemyDetector
    {
        public CircleFirstEnemyDetector(LayerMask enemyLayer) : base(enemyLayer) { }     

        public override Vector2? GetEnemyPosition(Vector2 origin, Vector2 direction = default, float maxDistance = 100)
        {
            Collider2D[] hits = TakeCircleHits(origin, maxDistance);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    return enemy.transform.position;
                }
            }

            return null;
        }
    }
}

