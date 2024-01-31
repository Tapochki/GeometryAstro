
using System;
using TandC.Data;
using TandC.Settings;
using UnityEngine;
using static TandC.Gameplay.EnemyBuilder;

namespace TandC.Gameplay 
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] Player _player;
        public Enemy CreateEnemy(EnemyData data, Enemy enemy, EnemyBuilderType type)
        {
            IEnemyBuilder builder = GetBuilder(type);        
            return builder.Build(enemy, data, _player);
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

