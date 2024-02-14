using System;
using TandC.Data;
using TandC.Settings;
using UnityEngine;

namespace TandC.Gameplay
{
    public interface IEnemyFactory
    {
        public Enemy CreateEnemy(EnemyData data, Enemy enemy, Action<Enemy> backToPoolEvent, Transform target, Vector2 direction, EnemyBuilderType type);
    }
}

