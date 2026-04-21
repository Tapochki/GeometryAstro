using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Detects enemies by layer mask. Returns the Transform of the nearest enemy in range,
    /// or null if none found.
    /// </summary>
    public interface IEnemyDetector
    {
        Transform GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100f);
    }
}
