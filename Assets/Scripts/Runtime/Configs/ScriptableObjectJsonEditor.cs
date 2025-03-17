using UnityEngine;
using UnityEditor;
using System.IO;

namespace TandC.GeometryAstro.ConfigUtilities 
{
    public interface IJsonSerializable { }

    [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectJsonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (target is IJsonSerializable)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Save to JSON"))
                {
                    SaveToJson(target);
                }

                if (GUILayout.Button("Load from JSON"))
                {
                    LoadFromJson(target);
                }
            }
        }

        private void SaveToJson(Object target)
        {
            string assetPath = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogError("Cannot determine asset path!");
                return;
            }

            string directory = Path.GetDirectoryName(assetPath);
            string fileName = $"{target.name}.json";
            string jsonPath = Path.Combine(directory, fileName);

            string json = JsonUtility.ToJson(target, true);
            File.WriteAllText(jsonPath, json);
            AssetDatabase.Refresh();

            Debug.Log($"Saved JSON to {jsonPath}");
        }

        private void LoadFromJson(Object target)
        {
            string assetPath = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogError("Cannot determine asset path!");
                return;
            }

            string directory = Path.GetDirectoryName(assetPath);
            string fileName = $"{target.name}.json";
            string jsonPath = Path.Combine(directory, fileName);

            if (!File.Exists(jsonPath))
            {
                Debug.LogError($"JSON file not found: {jsonPath}");
                return;
            }

            string json = File.ReadAllText(jsonPath);
            JsonUtility.FromJsonOverwrite(json, target);
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();

            Debug.Log($"Loaded JSON from {jsonPath}");
        }
    }
}


