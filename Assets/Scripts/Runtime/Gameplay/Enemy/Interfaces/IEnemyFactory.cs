﻿using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemyFactory
    {
        public Enemy CreateEnemy(EnemyData enemyData, Enemy enemy, Action<Enemy, bool> onDeathEvent, Transform target, Vector2 moveDirection, EnemyBuilderType builderType, float healthModificator, float speedModificator, float damageModificator, float sizeModificator = 1);
    }
}

