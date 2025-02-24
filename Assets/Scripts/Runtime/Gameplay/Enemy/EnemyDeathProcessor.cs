using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class EnemyDeathProcessor : IEnemyDeathProcessor
    {
        private IItemSpawner _itemSpawner;

        private void Construct(IItemSpawner enemySpawner)
        {
            _itemSpawner = enemySpawner;
        }

        public void EnemyDeathHandler(Enemy enemy)
        {
            _itemSpawner.DropRandomItem(enemy.EnemyData.droperType, enemy.transform.position);
        }
    }
}

