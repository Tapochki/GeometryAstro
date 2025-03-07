
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
            Debug.LogError("Origin" + origin);
            Debug.LogError("Direction" + direction);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction);

            Debug.DrawRay(origin, direction * maxDistance, Color.red, 0.1f);

            if(hit.collider.gameObject.TryGetComponent(out Enemy enemy)) 
            {
                return true;
            }

            return false;
        }
    }
}

