using UnityEditor;
using UnityEngine;

namespace Studio.Utilities.Editor
{
    [CustomEditor(typeof(RaycasterUtility))]
    public class RaycasterUtilityEditor : UnityEditor.Editor
    {
        private RaycasterUtility _target;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            InitTargetScript();

            if (GUILayout.Button("Start raycasting"))
            {
                _target.StartRaycastingObject();
            }
        }

        private void InitTargetScript()
        {
            _target = (RaycasterUtility)target;
        }
    }
}