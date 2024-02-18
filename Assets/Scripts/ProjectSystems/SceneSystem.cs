using System;
using System.Collections;
using TandC.Settings;
using TandC.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace TandC.ProjectSystems
{
    public class SceneSystem : MonoBehaviour
    {
        public event Action<SceneNames> OnSceneLoadedEvent;

        public event Action<float> OnSceneLoadingProgressEvent;

        private AsyncOperation _currentAsycnOperation;

        private SceneNames _currentSceneName = SceneNames.Unknown;
        private SceneNames _aimedAfterLoadingSceneName = SceneNames.Unknown;
        private SceneNames _sceneToLoadName = SceneNames.Unknown;

        private bool _isAutoOpen;
        private float _delayToOpenScene;

        private UISystem _uiSystem;

        [Inject]
        public void Construct(UISystem uiSystem)
        {
            Utilities.Logger.Log("SceneSystem Construct", LogTypes.Info);

            _uiSystem = uiSystem;
        }

        public void Initialize()
        {
            InternalTools.DoActionDelayed(() => LoadSceneByNameWithAutoOpen(SceneNames.Splash), 1.0f);
        }

        public void OpenLoadedScene()
        {
            if (_currentAsycnOperation == null)
            {
                Utilities.Logger.Log("Try to open scene that don't loaded", LogTypes.Error);
                return;
            }

            _isAutoOpen = false;

            _uiSystem.ResetViewsBeforeSceneChange();

            _currentAsycnOperation.allowSceneActivation = true;

            _currentSceneName = _sceneToLoadName;

            _sceneToLoadName = SceneNames.Unknown;
            _currentAsycnOperation = null;
        }

        public void LoadSceneByName(SceneNames sceneName, SceneNames aimedSceneNameAfterLoading = SceneNames.Unknown)
        {
            ReturnIfTargetSceneIsCurrentScene(sceneName);

            _isAutoOpen = false;
            _delayToOpenScene = 0;

            SetupSceneSettings(sceneName, aimedSceneNameAfterLoading);
        }

        public void LoadSceneByNameWithAutoOpen(SceneNames sceneName, SceneNames aimedSceneNameAfterLoading = SceneNames.Unknown,
                                                float delayToOpenScene = 0.3f)
        {
            ReturnIfTargetSceneIsCurrentScene(sceneName);
            Debug.LogError(sceneName);
            _isAutoOpen = true;
            _delayToOpenScene = delayToOpenScene;

            SetupSceneSettings(sceneName, aimedSceneNameAfterLoading);
        }

        private void SetupSceneSettings(SceneNames sceneName, SceneNames aimedSceneNameAfterLoading = SceneNames.Unknown)
        {
            _sceneToLoadName = sceneName;

            _aimedAfterLoadingSceneName = aimedSceneNameAfterLoading;

            StartCoroutine(LoadScene(sceneName.ToString()));
        }

        public void LoadAimedAfterLoadingScene()
        {
            _isAutoOpen = false;
            if (_aimedAfterLoadingSceneName == SceneNames.Unknown)
            {
                Utilities.Logger.Log("Aimed scene name after loading can't be Unknown!!!", LogTypes.Warning);
                return;
            }

            StartCoroutine(LoadScene(_aimedAfterLoadingSceneName.ToString()));

            _aimedAfterLoadingSceneName = SceneNames.Unknown;
        }

        private IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            _currentAsycnOperation = asyncOperation;
            _currentAsycnOperation.allowSceneActivation = false;

            while (!_currentAsycnOperation.isDone)
            {
                // for present loading like 1-100%
                OnSceneLoadingProgressEvent?.Invoke(_currentAsycnOperation.progress * 100f);

                if (_currentAsycnOperation.progress >= 0.9f)
                {
                    if (_isAutoOpen)
                    {
                        _uiSystem.ResetViewsBeforeSceneChange();
                        InternalTools.DoActionDelayed(() => _currentAsycnOperation.allowSceneActivation = true, _delayToOpenScene);
                    }

                    OnSceneLoadedEvent?.Invoke(InternalTools.EnumFromString<SceneNames>(sceneName));
                    break;
                }

                yield return null;
            }
        }

        private void ReturnIfTargetSceneIsCurrentScene(SceneNames targetScene)
        {
            if (targetScene == _currentSceneName)
            {
                Utilities.Logger.Log($"You`r try to open the same scene {targetScene}/{_currentSceneName}", LogTypes.Warning);
                return;
            }
        }
    }
}