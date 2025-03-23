using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IEnemyBuilder
    {
        Enemy Build(Enemy enemy, EnemyData data, Action<Enemy, bool> onDeathEvent, Transform target, Vector2 rotationDirection);
    }

    public abstract class EnemyBuilderBase : IEnemyBuilder
    {
        public Enemy Build(Enemy enemy, EnemyData data, Action<Enemy, bool> onDeathEvent,
            Transform target, Vector2 rotationDirection)
        {
            enemy.Initialize(data, target, onDeathEvent);
            ConfigureComponents(enemy, data, rotationDirection);
            return enemy;
        }

        protected abstract void ConfigureComponents(Enemy enemy, EnemyData data,
            Vector2 rotationDirection);
    }

    public class DefaultEnemyBuilder : EnemyBuilderBase
    {
        protected override void ConfigureComponents(Enemy enemy, EnemyData data, Vector2 rotationDirection)
        {
            var move = new MoveToTargetComponent(enemy.GetComponent<Rigidbody2D>());
            var rotation = new NoRotationComponent();
            var attack = new AttackComponent(data);
            rotation.SetRotation(rotationDirection);

            enemy.ConfigureComponents(move, rotation, attack);
        }

    }

    public class SawEnemyBuilder : EnemyBuilderBase
    {
        protected override void ConfigureComponents(Enemy enemy, EnemyData data, Vector2 rotationDirection)
        {
            var move = new MoveInDirectionComponent(enemy.GetComponent<Rigidbody2D>());
            var rotation = new OnTargetRotateComponte(enemy.transform);
            rotation.SetRotation(rotationDirection);
            var attack = new AttackComponent(data);

            enemy.ConfigureComponents(move, rotation, attack);
        }
    }
}

