using UnityEngine;
using Zenject;

namespace TandC.Gameplay 
{
    public class EnemyDeathProcessor : MonoBehaviour, IEnemyDeathProcessor
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
}

