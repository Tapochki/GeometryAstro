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
        }

        public override void Start()
        {
        }
    }
}