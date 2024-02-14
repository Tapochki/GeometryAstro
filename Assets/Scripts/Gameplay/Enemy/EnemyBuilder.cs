using System;
using TandC.Data;
using UnityEngine;

namespace TandC.Gameplay 
{
    public interface IEnemyBuilder
    {
        Enemy Build(Enemy enemy, EnemyData data, Action<Enemy> deathEvent, Transform target, Vector2 rotationDirection);
    }
    public class DefaultEnemyBuilder : IEnemyBuilder
    {
        public Enemy Build(Enemy enemy, EnemyData data, Action<Enemy> backToPoolEvent, Transform target, Vector2 rotationDirection)
        {
            enemy.transform.Find("ModelView").GetComponent<SpriteRenderer>().sprite = data.mainSprite;
            enemy.SetData(data);
            enemy.SetTargetDirection(target);
            enemy.SetTargetRotation(rotationDirection);
            enemy.SetBackToPoolEvent(backToPoolEvent);
            enemy.SetMovementComponent(new MoveToTargetComponent(enemy.GetComponent<Rigidbody2D>()));
            enemy.SetRotationComponent(new NoRotationComponent());
            enemy.SetHealthComponent(new HealthComponent(data.health, enemy.ProccesingEnemyDeath, null));
            enemy.SetAttackComponent(new AttackComponent(data));
            return enemy;
        }
    }

    public class SawEnemyBuilder : IEnemyBuilder
    {
        public Enemy Build(Enemy enemy, EnemyData data, Action<Enemy> backToPoolEvent, Transform target, Vector2 rotationDirection)
        {
            enemy.transform.Find("ModelView").GetComponent<SpriteRenderer>().sprite = data.mainSprite;
            Transform enemyModel = enemy.transform.Find("Model");
            enemyModel.GetComponent<SpriteRenderer>().sprite = data.enemyAdditionalSprite;
            enemy.SetData(data);
            enemy.SetTargetDirection(target);
            enemy.SetTargetRotation(rotationDirection);
            enemy.SetBackToPoolEvent(backToPoolEvent);
            enemy.SetMovementComponent(new MoveInDirectionComponent(enemy.GetComponent<Rigidbody2D>()));
            enemy.SetRotationComponent(new OnTargetRotateComponte(enemy.transform, rotationDirection));
            //   enemy.SetRotationComponent(new InfinitRotate(enemyModel));//Тут надо будет добавить компонент что-то тип анимации он есть у некоторых и будет отвечать за анимацию его модели через код у пилы собственно вращать ее
            enemy.SetHealthComponent(new HealthComponent(data.health, enemy.ProccesingEnemyDeath, null));
            enemy.SetAttackComponent(new AttackComponent(data));
            return enemy;
        }
    }
}

