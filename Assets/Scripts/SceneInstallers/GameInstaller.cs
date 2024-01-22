using ChebDoorStudio.Gameplay.Player;
using ChebDoorStudio.SceneSystems;
using ChebDoorStudio.ScriptableObjects;
using Zenject;

namespace ChebDoorStudio.SceneInstallers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PauseSystem>().FromComponentInHierarchy().AsCached();
            Container.Bind<PlayerComponent>().FromComponentInHierarchy().AsCached();
            Container.Bind<ScoreSystem>().FromComponentInHierarchy().AsCached();
            Container.Bind<ItemSpawnSystem>().FromComponentInHierarchy().AsCached();

            Container.Bind<InitialGameData>().FromNewScriptableObjectResource("Data/InitialGameData").AsCached();
        }

        public override void Start()
        {
        }
    }
}