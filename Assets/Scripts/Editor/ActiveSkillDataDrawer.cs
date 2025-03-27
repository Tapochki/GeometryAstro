using UnityEngine;
using UnityEditor;
using TandC.GeometryAstro.Data;

[CustomPropertyDrawer(typeof(ActiveSkillData))]
public class ActiveSkillDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty skillNameProp = property.FindPropertyRelative("SkillName");
        label.text = string.IsNullOrEmpty(skillNameProp.stringValue) ? label.text : skillNameProp.stringValue;

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}