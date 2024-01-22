using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Studio.Editor
{
    internal static class MenuItems
    {
        private static string _pathToBootScene = "Assets/Scenes/Boot.unity";

        #region project file hierarchy Utility

        [MenuItem("Utility/Editor/Delete Empty Folders")]
        public static void DeleteEmptyFolders()
        {
            ProcessDirectory(Directory.GetCurrentDirectory() + "/Assets");
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log("Delete Empty Folders Successful");
        }

        private static void ProcessDirectory(string startLocation)
        {
            foreach (string directory in Directory.GetDirectories(startLocation))
            {
                ProcessDirectory(directory);
                if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory);
                    File.Delete(directory + ".meta");
                    UnityEngine.Debug.Log(directory + " deleted");
                }
            }
        }

        [MenuItem("Utility/Editor/Capture Screenshot")]
        public static void CaptureScreenshot()
        {
            ScreenCapture.CaptureScreenshot($"Screenshot_{DateTime.UtcNow.ToFileTime()}.png");
        }

        #endregion project file hierarchy Utility

        #region Auto Saving Scenes in Editor

        private static readonly int MinutesDelay = 2;

        private static bool _isStop;

        [MenuItem("Utility/AutoSaverScene/Init Auto Saving")]
        public static void Init()
        {
            UnityEngine.Debug.Log(
                "Initialized Auto Saving! Be warning - if you hide editor, saving will stop automatically. You need to initialize auto saving again");
            _isStop = false;
            EditorCoroutine.Start(Save());
        }

        [MenuItem("Utility/AutoSaverScene/Stop Auto Saving")]
        public static void Stop()
        {
            UnityEngine.Debug.Log("Stop Auto Saving");
            _isStop = true;
        }

        private static IEnumerator Save()
        {
            int iterations = 60 * 60 * MinutesDelay; // frames count * seconds per minute * minutes count
            for (float i = 0; i < iterations; i++)
            {
                yield return null;
            }

            if (!_isStop)
            {
                UnityEngine.Debug.Log("Start Auto Save");
                if (EditorSceneManager.SaveOpenScenes())
                {
                    UnityEngine.Debug.Log("All Opened scenes was saved successfull!");
                }
                else
                {
                    UnityEngine.Debug.Log("Saving opened scenes failed");
                }

                EditorCoroutine.Start(Save());
            }
        }

        private class EditorCoroutine
        {
            private readonly IEnumerator _routine;

            private EditorCoroutine(IEnumerator routine)
            {
                _routine = routine;
            }

            public static EditorCoroutine Start(IEnumerator routine)
            {
                EditorCoroutine coroutine = new EditorCoroutine(routine);
                coroutine.Start();
                return coroutine;
            }

            public void Stop()
            {
                EditorApplication.update -= Update;
            }

            private void Start()
            {
                EditorApplication.update += Update;
            }

            private void Update()
            {
                if (!_routine.MoveNext())
                {
                    Stop();
                }
            }
        }

        #endregion Auto Saving Scenes in Editor

        #region cached data, player prefs, and data in persistent data path

        [MenuItem("Utility/Data/Delete PlayerPrefs")]
        public static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.Debug.Log("Delete Player Prefs Successful");
        }

        [MenuItem("Utility/Data/Clean Game Data (LocalLow)")]
        public static void CleanGameData()
        {
            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/");
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            DirectoryInfo[] directories = dir.GetDirectories();
            foreach (DirectoryInfo directory in directories)
            {
                directory.Delete(true);
            }

            UnityEngine.Debug.Log("Clean Game Data Successful");
        }

        [MenuItem("Utility/Data/Open Local Low Folder")]
        public static void OpenLocalLowFolder()
        {
            Process.Start(Application.persistentDataPath);
        }

        #endregion cached data, player prefs, and data in persistent data path

        #region asset bundles and cache

        [MenuItem("Utility/CacheAndBundles/Clean Cache")]
        public static void ClearCache()
        {
            if (Caching.ClearCache())
            {
                UnityEngine.Debug.Log("Clean Cache Successful");
            }
            else
            {
                UnityEngine.Debug.Log("Clean Cache Failed");
            }
        }

        [MenuItem("Utility/Build/Build Game")]
        public static void BuildGame()
        {
            BuildGame(EditorUserBuildSettings.activeBuildTarget);
        }

        private static void BuildGame(BuildTarget buildTarget)
        {
            string outputPath = Path.Combine("Builds", buildTarget.ToString());

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            outputPath = Path.Combine(outputPath, PlayerSettings.productName);
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    outputPath += ".exe";
                    break;

                case BuildTarget.Android:
                    outputPath += ".apk";
                    break;
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Select((scene, i) => scene.path).ToArray(),
                locationPathName = outputPath,
                target = buildTarget,
                targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget),
                options = BuildOptions.None
            };
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (buildReport.summary.result != BuildResult.Succeeded)
            {
                throw new Exception("build failed");
            }
        }

        #endregion asset bundles and cache

        #region scene hierarchy Utility

        [MenuItem("Utility/Editor/Select GOs With Missing Scripts")]
        [Obsolete]
        public static void SelectMissing(MenuCommand command)
        {
            Transform[] ts = Object.FindObjectsOfType<Transform>();
            List<GameObject> selection = new List<GameObject>();
            foreach (Transform t in ts)
            {
                Component[] cs = t.gameObject.GetComponents<Component>();
                foreach (Component c in cs)
                {
                    if (c == null)
                    {
                        selection.Add(t.gameObject);
                    }
                }
            }

            Selection.objects = selection.ToArray();
        }

        #endregion scene hierarchy Utility

        [MenuItem("Tools/Scenes/Open Boot Scene")]
        public static void OpenBootScene()
        {
            EditorSceneManager.OpenScene(_pathToBootScene);
        }

        [MenuItem("Tools/Scenes/Open Splash Scene")]
        public static void OpenSplashScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Splash.unity");
        }

        [MenuItem("Tools/Scenes/Open Loading Scene")]
        public static void OpenLoadingScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Loading.unity");
        }

        [MenuItem("Tools/Scenes/Open Menu Scene")]
        public static void OpenMenuScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");
        }

        [MenuItem("Tools/Scenes/Open Game Scene")]
        public static void OpenGameplayScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Gameplay.unity");
        }

        [MenuItem("Tools/Scenes/Set Boot scene default")]
        public static void DefaultScene()
        {
            SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(_pathToBootScene);

            EditorSceneManager.playModeStartScene = myWantedStartScene;
        }
    }
}