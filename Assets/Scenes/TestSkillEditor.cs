using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(TestSkill))]
public class TestSkillEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestSkill testSkill = (TestSkill)target;
        if (GUILayout.Button("Test Upgrade Skill"))
        {
            testSkill.SendMessage("TestUpgradeSkill");
        }
    }
}