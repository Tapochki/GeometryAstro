
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RaycastEnemyDetector : IEnemyDetector
    {
        private readonly LayerMask _enemyLayer;

        public RaycastEnemyDetector(LayerMask enemyLayer)
        {
            _enemyLayer = enemyLayer;
        }

        public bool HasEnemyInDirection(Vector2 origin, Vector2 direction, float maxDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, -(origin - direction), maxDistance, _enemyLayer);

            return hit.collider != null && hit.collider.TryGetComponent(out Enemy _);
        }
    }
}

