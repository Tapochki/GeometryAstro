#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;

namespace Studio.Utilities
{
    [CustomEditor(typeof(MultiComponentButton))]
    public class MultiComponentButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings");
            EditorGUILayout.Space();

            var target = (MultiComponentButton)base.target;
            target.title = EditorGUILayout.TextField("Title", target.title);
            target.icon = (UnityEngine.UI.Image)EditorGUILayout.ObjectField("Image", target.icon, typeof(UnityEngine.UI.Image), true);

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Images"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Texts"), true);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if (UnityEngine.GUILayout.Button("UpdateTitle"))
            {
                target.UpdateTitle();
            }

            EditorGUILayout.Space();

            if (!target.IsIconExist())
            {
                if (UnityEngine.GUILayout.Button("AddIcon"))
                {
                    target.AddIconObject();
                }
            }
        }
    }
}

#endif