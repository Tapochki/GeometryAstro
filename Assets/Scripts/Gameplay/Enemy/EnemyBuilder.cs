using TandC.Data;
using TandC.Gameplay;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class EnemyBuilder : MonoBehaviour
    {
        public interface IEnemyBuilder
        {
            Enemy Build(GameObject prefab, EnemyData data, Player player);
        }
        public class DefaultEnemyBuilder : IEnemyBuilder
        {
            public Enemy Build(GameObject prefab, EnemyData data, Player player)
            {
                SawEnemy enemy = Instantiate(prefab).AddComponent<SawEnemy>();
                enemy.SetData(data, player);
                enemy.SetMovementComponent(new MoveToTargetComponent(enemy.GetComponent<Rigidbody2D>()));
                enemy.SetRotationComponent(new NoRotationComponent());
                enemy.SetHealthComponent(new HealthComponent(data.health, null, null));
                return enemy;
            }
        }

        public class SawEnemyBuilder : IEnemyBuilder
        {
            public Enemy Build(GameObject prefab, EnemyData data, Player player)
            {
                SawEnemy enemy = Instantiate(prefab).AddComponent<SawEnemy>();
                enemy.SetData(data, player);
                enemy.SetMovementComponent(new MoveInDirectionComponent(enemy.GetComponent<Rigidbody2D>()));
                enemy.SetRotationComponent(new InfinitRotate(enemy.gameObject.transform));//Поменять на модль пилы а то модель будет вращатся а оно нам нахуй не нужно
                enemy.SetHealthComponent(new HealthComponent(data.health, null, null));
                return enemy;
            }
        }
    }
}

