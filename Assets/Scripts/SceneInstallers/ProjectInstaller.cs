using TandC.ProjectSystems;
using UnityEngine;
using Zenject;

namespace TandC
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadObjectsSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<InputsSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LocalisationSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SoundSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DataSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UISystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SceneSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AdvertismentSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PurchasingSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<VaultSystem>().FromComponentInHierarchy().AsSingle();

            Container.Bind<GameStateSystem>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<GameClient>().AsSingle();
        }

        public override void Start()
        {
            Application.targetFrameRate = 60;
        }
    }

    public sealed class GameClient : IInitializable
    {
        private LoadObjectsSystem _loadObjectsSystem;
        private InputsSystem _inputsSystem;
        private LocalisationSystem _localisationSystem;
        private SoundSystem _soundSystem;
        private DataSystem _dataSystem;
        private UISystem _uiSystem;
        private SceneSystem _sceneSystem;
        private AdvertismentSystem _advertismentSystem;
        private PurchasingSystem _purchasingSystem;
        private VaultSystem _vaultSystem;
        private GameStateSystem _gameStateSystem;

        [Inject]
        public void Construct(
                                LoadObjectsSystem loadObjectsSystem,
                                InputsSystem inputsSystem,
                                LocalisationSystem localisationSystem,
                                SoundSystem soundSystem,
                                DataSystem dataSystem,
                                UISystem uiSystem,
                                SceneSystem sceneSystem,
                                AdvertismentSystem advertismentSystem,
                                PurchasingSystem purchasingSystem,
                                VaultSystem vaultSystem,
                                GameStateSystem gameStateSystem)
        {
            _loadObjectsSystem = loadObjectsSystem;
            _inputsSystem = inputsSystem;
            _localisationSystem = localisationSystem;
            _soundSystem = soundSystem;
            _dataSystem = dataSystem;
            _uiSystem = uiSystem;
            _sceneSystem = sceneSystem;
            _advertismentSystem = advertismentSystem;
            _purchasingSystem = purchasingSystem;
            _vaultSystem = vaultSystem;
            _gameStateSystem = gameStateSystem;
        }

        public void Initialize()
        {
            _loadObjectsSystem.Initialize();
            _inputsSystem.Initialize();
            _localisationSystem.Initialize();
            _soundSystem.Initialize();
            _dataSystem.Initialize();
            _uiSystem.Initialize();
            _sceneSystem.Initialize();
            _advertismentSystem.Initialize();
            _purchasingSystem.Initialize();
            _vaultSystem.Initialize();
            _gameStateSystem.Initialize();
        }
    }
}