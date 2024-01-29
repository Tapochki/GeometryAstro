
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
        public Enemy CreateEnemy(EnemyData data, EnemyType type)
        {
            IEnemyBuilder builder = GetBuilder(type);
            GameObject prefab = data.enemyPrefab;
            
            return builder.Build(prefab, data, _player);
        }

        private IEnemyBuilder GetBuilder(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.StandartSquare:
                    return new DefaultEnemyBuilder();
                case EnemyType.Saw:
                    return new SawEnemyBuilder();
                default:
                    throw new ArgumentException("Unsupported enemy type");
            }
        }
    }
}

