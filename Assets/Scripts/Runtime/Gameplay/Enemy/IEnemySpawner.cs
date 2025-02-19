using TandC.GeometryAstro.Data;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemySpawner
    {
        public void StartWave(EnemySpawnData[] enemyDatas, float spawnDelay);
    }
}

