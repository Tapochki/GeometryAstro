using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "TandC/Game/EnemyConfig", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private List<EnemyData> _enemies;

        public EnemyData GetEnemiesByType(EnemyType type)
        {
            foreach (var item in _enemies)
            {
                if (item.type == type)
                {
                    return item;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class EnemyData
    {
        public int enemyId;
        public float health;
        public int damage;
        public float movementSpeed;
        public EnemyBuilderType BuilderType;
        public EnemyType type;
        public DropItemRareType droperType;
        public Sprite mainSprite;
        public Sprite enemyAdditionalSprite;
    }
}