using Studio.Utilities;
using UnityEditor;
using UnityEngine;

namespace RomanMalitskyi.Utilities.Editor
{
    [CustomEditor(typeof(ShadowedTextMexhProUGUI))]
    public class ShadowedTextMexhProUGUIEditor : UnityEditor.Editor
    {
        private GUIStyle _defaultStyle;
        private ShadowedTextMexhProUGUI _target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            InitScriptTarget();

            if (!_target.GetShadowInitState())
            {
                _target.SetIsHaveShadow(EditorGUILayout.Toggle("Is Have Shadow", _target.GetIsHaveShadow()));
            }

            EditorGUILayout.Space();
            if (_target.GetShadowInitState())
            {
                AddShadowSettings();
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

                if (!_target.GetShadowInitState())
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
                        EditorGUILayout.LabelField("Make sure the padding is large enough to show the shadow.");
                        EditorGUILayout.Space();
                        if (GUILayout.Button("Init shadow"))
                        {
                            _target.AddShadowToText();
                        }
                    }
                    else
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Refresh shadow"))
                        {
                            _target.RefreshShadow();
                        }

                        if (GUILayout.Button("Remove shadow"))
                        {
                            _target.RemoveShadow();
                            _target.SetIsHaveShadow(false);
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

        private void InitScriptTarget()
        {
            _target = (ShadowedTextMexhProUGUI)target;
        }

        private void InitStyle()
        {
            _defaultStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,

                normal = new GUIStyleState()
                {
                    background = Texture2D.grayTexture,
                    textColor = Color.white
                },
                hover = new GUIStyleState()
                {
                    background = Texture2D.whiteTexture,
                    textColor = Color.white
                },
                active = new GUIStyleState()
                {
                    background = Texture2D.blackTexture,
                    textColor = Color.white
                }
            };
        }

        private Texture2D MakeBackgroundTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D backgroundTexture = new Texture2D(width, height);

            backgroundTexture.SetPixels(pixels);
            backgroundTexture.Apply();

            return backgroundTexture;
        }
    }
}