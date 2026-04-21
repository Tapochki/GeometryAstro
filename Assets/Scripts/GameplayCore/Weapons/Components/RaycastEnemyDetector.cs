using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Detects the first enemy along a forward raycast.
    /// Enemies must be on the layer specified in the constructor (e.g. LayerMask.GetMask("Enemy")).
    /// </summary>
    public class RaycastEnemyDetector : IEnemyDetector
    {
        private readonly LayerMask _enemyLayer;

        public RaycastEnemyDetector(LayerMask enemyLayer)
        {
            _enemyLayer = enemyLayer;
        }

        public Transform GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100f)
        {
            Vector2 rayDir = direction == Vector2.zero ? Vector2.up : -(origin - direction).normalized;
            RaycastHit2D hit = Physics2D.Raycast(origin, rayDir, maxDistance, _enemyLayer);

            Debug.DrawRay(origin, rayDir * maxDistance, Color.red, 0.1f);

            if (hit.collider != null && hit.collider.TryGetComponent(out IDamageable _))
                return hit.collider.transform;
            return null;
        }
    }
}
