using Studio.SceneSystems;
using Studio.ScriptableObjects;
using Zenject;

namespace Studio.SceneInstallers
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