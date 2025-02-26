using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemySpawnPositionService
    {
        public void Init();

        public Transform GetRandomPositionFromRegister();

        public Vector2 GetOppositePosition(Vector2 spawnPosition);
    }
}


