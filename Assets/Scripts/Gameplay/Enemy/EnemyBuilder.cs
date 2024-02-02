using TandC.Data;
using UnityEngine;

namespace TandC.Gameplay
{
    public class EnemyBuilder : MonoBehaviour
    {
        public interface IEnemyBuilder
        {
            Enemy Build(Enemy enemy, EnemyData data, Player player);
        }

        public class DefaultEnemyBuilder : IEnemyBuilder
        {
            public Enemy Build(Enemy enemy, EnemyData data, Player player)
            {
                enemy.GetComponent<SpriteRenderer>().sprite = data.mainSprite;
                enemy.SetData(data, player);
                enemy.SetMovementComponent(new MoveToTargetComponent(enemy.GetComponent<Rigidbody2D>()));
                enemy.SetRotationComponent(new NoRotationComponent());
                enemy.SetHealthComponent(new HealthComponent(data.health, null, null));
                return enemy;
            }
        }

        public class SawEnemyBuilder : IEnemyBuilder
        {
            public Enemy Build(Enemy enemy, EnemyData data, Player player)
            {
                enemy.GetComponent<SpriteRenderer>().sprite = data.mainSprite;
                Transform enemyModel = enemy.transform.Find("Model");
                enemyModel.GetComponent<SpriteRenderer>().sprite = data.enemyAdditionalSprite;
                enemy.SetData(data, player);
                enemy.SetMovementComponent(new MoveInDirectionComponent(enemy.GetComponent<Rigidbody2D>()));
                enemy.SetRotationComponent(new InfinitRotate(enemyModel));//Поменять на модль пилы а то модель будет вращатся а оно нам нахуй не нужно
                enemy.SetHealthComponent(new HealthComponent(data.health, null, null));
                return enemy;
            }
        }
    }
}