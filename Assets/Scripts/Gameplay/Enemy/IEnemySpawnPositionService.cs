using System;
using System.Collections.Generic;
using System.Linq;
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


