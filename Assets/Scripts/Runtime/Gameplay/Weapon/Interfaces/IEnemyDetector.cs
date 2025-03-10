using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemyDetector
    {
        public Vector2? GetEnemyPosition(Vector2 origin, Vector2 direction = default, float maxDistance = 100);
    }
}
