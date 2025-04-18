
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class CircleFirstEnemyDetector : CircleEnemyDetector
    {
        public CircleFirstEnemyDetector(LayerMask enemyLayer) : base(enemyLayer) { }     

        public override Enemy GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100)
        {
            Collider2D[] hits = TakeCircleHits(origin, maxDistance);

            Enemy enemy = HitScanEnemy(hits);

            return enemy;
        }
    }
}

