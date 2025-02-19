using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemyFactory
    {
        public Enemy CreateEnemy(EnemyData data, Enemy enemy, Action<Enemy> backToPoolEvent, Transform target, Vector2 direction, EnemyBuilderType type);
    }
}

