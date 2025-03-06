using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TandC.GeometryAstro.Editor
{
    [InitializeOnLoad]
    public static class BootSceneAutoLoader
    {
        private const string TEST_CURRENT_KEY = "TestCurrentSceneEnabled";

        static BootSceneAutoLoader()
        {
            EditorBuildSettings.sceneListChanged += SetStartScene;
            EditorSceneManager.activeSceneChangedInEditMode += OnActiveSceneChanged;

            SetStartScene();
            UpdateMenuCheckedState();
        }

        [MenuItem("Tools/Toggle Test Current Scene")]
        private static void ToggleTestCurrentScene()
        {
            bool currentState = EditorPrefs.GetBool(TEST_CURRENT_KEY, false);
            EditorPrefs.SetBool(TEST_CURRENT_KEY, !currentState);
            SetStartScene();
            UpdateMenuCheckedState();
        }

        private static void UpdateMenuCheckedState()
        {
            UnityEditor.Menu.SetChecked("Tools/Toggle Test Current Scene",
                          EditorPrefs.GetBool(TEST_CURRENT_KEY, false));
        }

        private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (EditorPrefs.GetBool(TEST_CURRENT_KEY, false))
            {
                SetStartScene();
            }
        }

        private static void SetStartScene()
        {
            if (EditorPrefs.GetBool(TEST_CURRENT_KEY, false))
            {
                SetCurrentSceneAsStart();
            }
            else
            {
                SetBootSceneAsStart();
            }
        }

        private static void SetCurrentSceneAsStart()
        {
            Scene activeScene = EditorSceneManager.GetActiveScene();
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(activeScene.path);

            if (sceneAsset == null)
            {
                Debug.LogWarning("Current scene is not saved! Start scene reset.");
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            EditorSceneManager.playModeStartScene = sceneAsset;
            Debug.Log($"Play mode will start with CURRENT scene: {activeScene.name}");
        }

        private static void SetBootSceneAsStart()
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.LogError("Build settings are empty!");
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            SceneAsset bootSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(
                EditorBuildSettings.scenes[0].path);

            if (!bootSceneAsset.name.Contains("Boot"))
            {
                Debug.LogError("First scene in build settings must be Boot!");
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            EditorSceneManager.playModeStartScene = bootSceneAsset;
            Debug.Log("Play mode will start with BOOT scene");
        }
    }
}

