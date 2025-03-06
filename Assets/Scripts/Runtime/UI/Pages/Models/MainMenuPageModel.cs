
using Cysharp.Threading.Tasks;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.UI
{
    public class MainMenuPageModel
    {
        private LoadObjectsService _loadObjectsService;
        private SceneService _sceneService;
        private UIService _uiService;

        private GameObject _selfObject;

        public MainMenuPageModel(
            LoadObjectsService loadObjectsService,
            SceneService sceneService,
            UIService uiService)
        {
            _sceneService = sceneService;
            _loadObjectsService = loadObjectsService;
            _uiService = uiService;
        }

        public GameObject GetSelfObject()
        {
            if (_selfObject == null)
            {
                _selfObject = SpawnPrefab();
                return _selfObject;
            }
            else
            {
                return _selfObject;
            }
        }

        private GameObject SpawnPrefab()
        {
            return MonoBehaviour.Instantiate(FindPrefab(), _uiService.Canvas.transform);
        }

        private GameObject FindPrefab()
        {
            return _loadObjectsService.GetObjectByPath<GameObject>("UI/Pages/MainMenuPage");
        }

        public void LoadGameScene()
        {
            _sceneService.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}