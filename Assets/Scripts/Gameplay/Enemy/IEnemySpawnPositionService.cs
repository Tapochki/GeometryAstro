using System.Collections.Generic;
using TandC.Settings;
using UnityEngine;

namespace TandC.Gameplay
{
    public interface IEnemySpawnPositionService
    {
        public List<Transform> GetSpawnPointsForType(SpawnType spawnType);

        public Vector2 GetOppositePosition(Vector2 spawnPosition);
    }
}


