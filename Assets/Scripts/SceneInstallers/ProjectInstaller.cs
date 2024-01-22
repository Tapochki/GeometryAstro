using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.Utilities;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LoadObjectsSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<InputsSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<LocalisationSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<SoundSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<DataSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<UISystem>().AsCached();
            Container.BindInterfacesAndSelfTo<SceneSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<AdvertismentSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<PurchasingSystem>().AsCached();
            Container.BindInterfacesAndSelfTo<VaultSystem>().AsCached();

            Container.BindInterfacesAndSelfTo<GameStateSystem>().AsCached();

            Container.Bind<MonoHelper>().FromComponentInHierarchy().AsCached();
        }

        public override void Start()
        {
            Application.targetFrameRate = 60;
        }
    }

    public sealed class GameClient
    {
    }
}