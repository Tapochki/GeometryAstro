using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using TandC.GeometryAstro.Data;

[CustomPropertyDrawer(typeof(PlayerParam))]
public class PlayerParamDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty typeProperty = property.FindPropertyRelative("Type");
        label.text = typeProperty.enumDisplayNames[typeProperty.enumValueIndex]; // Отображение Type вместо Element

        EditorGUI.PropertyField(position, property, label, true);
    }
}