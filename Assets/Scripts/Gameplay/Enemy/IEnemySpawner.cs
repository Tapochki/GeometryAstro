using TandC.Data;

namespace TandC.Gameplay
{
    public interface IEnemySpawner
    {
        public void StartWave(EnemySpawnData[] enemyDatas, float spawnDelay);
    }
}

