
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

        public Vector2? GetEnemyPosition(Vector2 origin, Vector2 direction = default, float maxDistance = 100)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, -(origin - direction), maxDistance, _enemyLayer);

            Debug.DrawRay(origin, -(origin - direction) * maxDistance, Color.red, 0.1f);

            if (hit.collider != null && hit.collider.TryGetComponent(out Enemy enemy))
            {
                return enemy.transform.position;
            }
            return null;
        }
    }
}

