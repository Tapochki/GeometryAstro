using System.Collections;
using System.Collections.Generic;
using TandC.Gameplay;
using UnityEngine;
using Zenject;

public class EnemyDeathProcessor : MonoBehaviour
{
    private ItemSpawner _itemSpawner;
    [Inject]
    private void Construct(ItemSpawner enemySpawner)
    {
        _itemSpawner = enemySpawner;
    }
    public void EnemyDeathHandler(Enemy enemy) 
    {
        _itemSpawner.DropItem(enemy.EnemyData.droperType, enemy.transform.position);
    }
}
