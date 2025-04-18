using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemyDetector
    {
        public Enemy GetEnemy(Vector2 origin, Vector2 direction = default, float maxDistance = 100);
    }
}
