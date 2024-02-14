using System;
using TandC.Data;
using TandC.Settings;
using UnityEngine;

namespace TandC.Gameplay
{
    public class EnemyFactory : MonoBehaviour, IEnemyFactory
    {
        public Enemy CreateEnemy(EnemyData data, Enemy enemy, Action<Enemy> backToPoolEvent, Transform target, Vector2 direction, EnemyBuilderType type)
        {
            IEnemyBuilder builder = GetBuilder(type);        
            return builder.Build(enemy, data, backToPoolEvent, target, direction);
        }

        private IEnemyBuilder GetBuilder(EnemyBuilderType type)
        {
            switch (type)
            {
                case EnemyBuilderType.Default:
                    return new DefaultEnemyBuilder();
                case EnemyBuilderType.Saw:
                    return new SawEnemyBuilder();
                default:
                    throw new ArgumentException("Unsupported enemy type");
            }
        }
    }
}

