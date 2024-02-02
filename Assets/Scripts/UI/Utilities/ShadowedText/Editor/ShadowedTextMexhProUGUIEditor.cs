using UnityEditor;
using UnityEngine;

namespace TandC.Utilities.Editor
{
    [CustomEditor(typeof(ShadowedTextMexhProUGUI))]
    public class ShadowedTextMexhProUGUIEditor : UnityEditor.Editor
    {
        private ShadowedTextMexhProUGUI _target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            InitScriptTarget();

            if (!_target.GetShadowInitState())
            {
                _target.SetIsHaveShadow(EditorGUILayout.Toggle("Is Have Shadow", _target.GetIsHaveShadow()));
            }

            if (!_target.GetHighlightInitState())
            {
                _target.SetIsHaveHighlight(EditorGUILayout.Toggle("Is Have Highlight", _target.GetIsHaveHighlight()));
            }

            EditorGUILayout.Space();
            if (_target.GetShadowInitState())
            {
                AddShadowSettings();
            }
            EditorGUILayout.Space();
            if (_target.GetHighlightInitState())
            {
                AddHighlightSettings();
            }

            EditorGUILayout.Space();
            if (!_target.GetInitState() && _target.GetFontAsset() != null)
            {
                if (GUILayout.Button("Init text"))
                {
                    _target.InitText();
                }
            }

            if (_target.GetInitState())
            {
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Refresh text"))
                {
                    _target.RefreshText();
                }

                if (!_target.GetShadowInitState() || !_target.GetHighlightInitState())
                {
                    if (GUILayout.Button("Remove text"))
                    {
                        _target.RemoveText();
                    }
                }

                GUILayout.EndHorizontal();

                if (_target.GetIsHaveShadow())
                {
                    if (!_target.GetShadowInitState())
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Make sure the padding is large enough to show the Shadow.");
                        EditorGUILayout.Space();
                        if (GUILayout.Button("Init Shadow"))
                        {
                            _target.AddShadowToText();
                        }
                    }
                    else
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Refresh Shadow"))
                        {
                            _target.RefreshShadow();
                        }

                        if (GUILayout.Button("Remove Shadow"))
                        {
                            _target.RemoveShadow();
                            _target.SetIsHaveShadow(false);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.Space();
                if (_target.GetIsHaveHighlight())
                {
                    if (!_target.GetHighlightInitState())
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Make sure the padding is large enough to show the highlight.");
                        EditorGUILayout.Space();
                        if (GUILayout.Button("Init Highlight"))
                        {
                            _target.AddHighlightToText();
                        }
                    }
                    else
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Refresh Highlight"))
                        {
                            _target.RefreshHighlight();
                        }

                        if (GUILayout.Button("Remove Highlight"))
                        {
                            _target.RemoveHighlight();
                            _target.SetIsHaveHighlight(false);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }

        private void AddShadowSettings()
        {
            _target.ShadowColor = EditorGUILayout.ColorField("Shadow Color", _target.ShadowColor);
            _target.ShadowThsickness = EditorGUILayout.Slider("Shadow Thsickness", _target.ShadowThsickness, 0f, 1f);
            _target.ShadowOffset = EditorGUILayout.Vector2Field("Shadow Offset", _target.ShadowOffset);
        }

        private void AddHighlightSettings()
        {
            _target.HighlightColor = EditorGUILayout.ColorField("Highlight Color", _target.HighlightColor);
            _target.HighlightThsickness = EditorGUILayout.Slider("Highlight Thsickness", _target.HighlightThsickness, 0f, 1f);
            _target.HighlightOffset = EditorGUILayout.Vector2Field("Highlight Offset", _target.HighlightOffset);
        }

        private void InitScriptTarget()
        {
            _target = (ShadowedTextMexhProUGUI)target;
        }
    }
}