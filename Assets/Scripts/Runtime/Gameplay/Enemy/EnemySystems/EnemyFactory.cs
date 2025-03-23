using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class EnemyFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(EnemyData data, Enemy enemy, Action<Enemy, bool> onDeathEvent, Transform target, Vector2 direction, EnemyBuilderType type, float healthModificator, float speedModificator, float damageModificator, float sizeModificator = 1)
        {
            IEnemyBuilder builder = GetBuilder(type);        
            return builder.Build(enemy, data, onDeathEvent, target, direction, healthModificator, speedModificator, damageModificator, sizeModificator);
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

