using TandC.GeometryAstro.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillConfig))]
public class SkillConfigScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (SkillConfig)target;

        GUILayout.Space(20);

        if (GUILayout.Button("Set Upgrade Description like main skill", (GUILayout.Height(40))))
        {
            script.SetUpgradeDescription();
        }
    }
}