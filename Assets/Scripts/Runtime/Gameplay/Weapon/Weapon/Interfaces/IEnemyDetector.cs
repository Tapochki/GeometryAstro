using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemyDetector
    {
        bool HasEnemyInDirection(Vector2 origin, Vector2 direction, float maxDistance);
    }
}
