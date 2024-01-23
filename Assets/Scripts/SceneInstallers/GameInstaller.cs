using TandC.SceneSystems;
using Zenject;

namespace TandC.SceneInstallers
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