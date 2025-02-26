using TandC.GeometryAstro.Data;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IEnemySpawner
    {
        public int ActiveEnemyCount { get; }

        public void Init();
        public void StartWave(EnemySpawnData[] enemyDatas);
        public void SpawnEnemy();
    }
}

