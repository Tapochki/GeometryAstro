using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemySpawnPositionService
    {
        public List<Transform> GetSpawnPointsForType(SpawnType spawnType);

        public Vector2 GetOppositePosition(Vector2 spawnPosition);
    }
}


