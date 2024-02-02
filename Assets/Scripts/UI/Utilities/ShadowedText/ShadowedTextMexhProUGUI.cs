using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TandC.Utilities
{
    public class ShadowedTextMexhProUGUI : MonoBehaviour
    {
        private GameObject _selfObject;

        [Header("Shadowed Text Rootname")]
        [SerializeField] private string _rootName;

        [Space(10)]
        private GameObject _mainTextObject;

        [HideInInspector] public TextMeshProUGUI MainText;

        private GameObject _shadowTextObject;
        [HideInInspector] public TextMeshProUGUI ShadowText;

        private GameObject _highlightTextObject;
        [HideInInspector] public TextMeshProUGUI HighlightText;

        private bool _initialized;
        private bool _shadowInitialized;
        private bool _highLightInitialized;

        #region Text Settings

        #region Main Text

        [Header("SETTINGS")]
        [TextArea()]
        [SerializeField] private string _textValue = "Lorem Ipsum Dolore";

        [SerializeField] private TMP_FontAsset _fontAsset;
        [SerializeField] private FontStyles _fontStyles;
        [SerializeField] private TextAlignmentOptions _alignmentOptions = TextAlignmentOptions.Center;
        [SerializeField] private float _textSizeMax = 24;
        [SerializeField] private float _textSizeMin = 8;
        [SerializeField] private float _leftTextPadding = 8f;
        [SerializeField] private float _rightTextPadding = 8f;

        [Space(5)]
        [SerializeField] private bool _autoSize = true;

        [SerializeField] private bool _isRaycast = false;
        [SerializeField] private bool _isWrapping = false;
        [SerializeField] private Color _mainColor = Color.white;
        [SerializeField] private TMP_SpriteAsset _spriteAsset;
        private bool _isHaveShadow;
        private bool _isHaveHighlight;

        #endregion Main Text

        #region Shadow Text

        [HideInInspector] public Color ShadowColor = Color.black;
        [HideInInspector] public float ShadowThsickness = 0.5f;
        [HideInInspector] public Vector3 ShadowOffset;

        #endregion Shadow Text

        #region Highlight Text

        [HideInInspector] public Color HighlightColor = Color.black;

        [HideInInspector] public float HighlightThsickness = 0.5f;
        [HideInInspector] public Vector3 HighlightOffset;

        #endregion Highlight Text

        #endregion Text Settings

        public bool GetInitState() => _initialized;

        public bool GetShadowInitState() => _shadowInitialized;

        public bool GetIsHaveShadow() => _isHaveShadow;

        public void SetIsHaveShadow(bool value) => _isHaveShadow = value;

        public bool GetHighlightInitState() => _highLightInitialized;

        public bool GetIsHaveHighlight() => _isHaveHighlight;

        public void SetIsHaveHighlight(bool value) => _isHaveHighlight = value;

        public TMP_FontAsset GetFontAsset() => _fontAsset;

        public TMP_SpriteAsset GetSpriteAsset() => _spriteAsset;

        #region Editor Methods

        #region Main Text

        public void InitText()
        {
            InitTextVariables();

            SetIsHaveShadow(IsShadowTextExist());

            if (GetIsHaveShadow())
            {
                RefreshShadow();
            }

            SetIsHaveHighlight(IsHighlightTextExist());

            if (GetIsHaveHighlight())
            {
                RefreshHighlight();
            }

            SetupFontSettings(MainText);
            MainText.color = _mainColor;
            MainText.raycastTarget = _isRaycast;

            SetInitState(true);

            _selfObject.name = "ShadowedText_" + _rootName;
        }

        public void RefreshText()
        {
            List<GameObject> childs = new List<GameObject>();

            for (int i = 0; i < _mainTextObject.transform.childCount; i++)
            {
                GameObject child = _mainTextObject.transform.GetChild(i).gameObject;

                if (child != null)
                {
                    child.transform.SetParent(_selfObject.transform);
                    childs.Add(child);
                }
            }

            RemoveText();

            InitText();

            for (int i = 0; i < childs.Count; i++)
            {
                GameObject child = childs[i];

                if (child != null)
                {
                    child.transform.SetParent(_mainTextObject.transform);
                    childs.Remove(child);
                }
            }
        }

        public void RemoveText()
        {
            SetInitState(false);

            DestroyImmediate(_mainTextObject);
            MainText = null;
        }

        public void AddShadowToText()
        {
            InitShadowTextVariables();

            SetupFontSettings(ShadowText);

            ShadowText = UpdatePreferencesInTextObject(ShadowText, ShadowColor, ShadowThsickness) as TextMeshProUGUI;

            _shadowTextObject.transform.localPosition = ShadowOffset;

            _shadowInitialized = true;
        }

        public void AddHighlightToText()
        {
            InitHighlightTextVariables();

            SetupFontSettings(HighlightText);

            HighlightText = UpdatePreferencesInTextObject(HighlightText, HighlightColor, HighlightThsickness) as TextMeshProUGUI;

            _highlightTextObject.transform.localPosition = HighlightOffset;

            _highLightInitialized = true;
        }

        private TMP_Text UpdatePreferencesInTextObject(TextMeshProUGUI textObject, Color color, float thsickness)
        {
            TMP_Text text = textObject;

            text.color = color;
            text.raycastTarget = false;
            text.outlineWidth = thsickness;
            text.outlineColor = color;
            return text;
        }

        public bool IsMainTextExist() => GetSelfObject().transform.Find("Text_Main")?.gameObject != null;

        public bool IsShadowTextExist() => GetSelfObject().transform.Find("Text_Shadow")?.gameObject != null;

        public bool IsHighlightTextExist() => GetSelfObject().transform.Find("Text_Highlight")?.gameObject != null;

        #endregion Main Text

        #region Highlight Text

        public void RefreshHighlight()
        {
            List<GameObject> childs = new List<GameObject>();
            var hightlightObject = GetSelfObject().transform.Find("Text_Highlight").gameObject;

            for (int i = 0; i < hightlightObject.transform.childCount; i++)
            {
                GameObject child = hightlightObject.transform.GetChild(i).gameObject;

                if (child != null)
                {
                    child.transform.SetParent(_selfObject.transform);
                    childs.Add(child);
                }
            }

            RemoveHighlight();

            AddHighlightToText();

            for (int i = 0; i < childs.Count; i++)
            {
                GameObject child = childs[i];

                if (child != null)
                {
                    child.transform.SetParent(_highlightTextObject.transform);
                    childs.Remove(child);
                }
            }
        }

        public void RemoveHighlight()
        {
            _highLightInitialized = false;

            DestroyImmediate(_highlightTextObject);
            HighlightText = null;
        }

        #endregion Highlight Text

        #region Shadow Text

        public void RefreshShadow()
        {
            List<GameObject> childs = new List<GameObject>();
            var shadowObject = GetSelfObject().transform.Find("Text_Shadow").gameObject;

            for (int i = 0; i < shadowObject.transform.childCount; i++)
            {
                GameObject child = shadowObject.transform.GetChild(i).gameObject;

                if (child != null)
                {
                    child.transform.SetParent(_selfObject.transform);
                    childs.Add(child);
                }
            }

            RemoveShadow();

            AddShadowToText();

            for (int i = 0; i < childs.Count; i++)
            {
                GameObject child = childs[i];

                if (child != null)
                {
                    child.transform.SetParent(_shadowTextObject.transform);
                    childs.Remove(child);
                }
            }
        }

        public void RemoveShadow()
        {
            _shadowInitialized = false;

            DestroyImmediate(_shadowTextObject);
            ShadowText = null;
        }

        #endregion Shadow Text

        #endregion Editor Methods

        private void Awake()
        {
            _selfObject = GetSelfObject();

            _mainTextObject = _selfObject.transform.Find("Text_Main").gameObject;
            _shadowTextObject = _selfObject.transform.Find("Text_Shadow").gameObject;
            _highlightTextObject = _selfObject.transform.Find("Text_Highlight").gameObject;

            MainText = _mainTextObject.GetComponent<TextMeshProUGUI>();
            ShadowText = _shadowTextObject.GetComponent<TextMeshProUGUI>();
            HighlightText = _highlightTextObject.GetComponent<TextMeshProUGUI>();
        }

        private void SetupFontSettings(TextMeshProUGUI targetText)
        {
            targetText.font = _fontAsset;
            targetText.fontSizeMax = _textSizeMax;
            targetText.fontSizeMin = _textSizeMin;
            targetText.fontStyle = _fontStyles;
            targetText.enableAutoSizing = _autoSize;
            targetText.textWrappingMode = _isWrapping ? TextWrappingModes.Normal : TextWrappingModes.NoWrap;
            targetText.margin = new Vector4(_leftTextPadding, 0, _rightTextPadding, 0);
            targetText.alignment = _alignmentOptions;
            targetText.text = _textValue;
            if (_spriteAsset != null)
            {
                targetText.spriteAsset = _spriteAsset;
            }
        }

        private void InitTextVariables()
        {
            _selfObject = null;
            _selfObject = GetSelfObject();

            Transform textObject = _selfObject.transform.Find("Text_Main");
            if (textObject == null)
            {
                _mainTextObject = new GameObject("Text_Main");
                _mainTextObject.transform.parent = _selfObject.transform;
                _mainTextObject.transform.localScale = Vector3.one;
                _mainTextObject.transform.localPosition = Vector3.zero;

                _mainTextObject.AddComponent<RectTransform>();

                UpdateTextObjectPositionAndAnchors(_mainTextObject);
            }
            else
            {
                _mainTextObject = textObject.gameObject;
            }

            if (_mainTextObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                MainText = text;
            }
            else
            {
                MainText = _mainTextObject.AddComponent<TextMeshProUGUI>();
            }
        }

        private TextMeshProUGUI InitAditionalText(GameObject textObject, string textObjectName)
        {
            _selfObject = null;
            _selfObject = GetSelfObject();

            if (textObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI textValue))
            {
                return textValue;
            }
            else
            {
                return textObject.AddComponent<TextMeshProUGUI>();
            }
        }

        private GameObject CreateTextObject(string textObjectName)
        {
            Transform text = _selfObject.transform.Find(textObjectName);
            GameObject textObject;
            if (text == null)
            {
                textObject = new GameObject(textObjectName);
                textObject.transform.parent = _selfObject.transform;
                textObject.transform.localScale = Vector3.one;
                textObject.transform.localPosition = Vector3.zero;

                textObject.transform.SetAsFirstSibling();

                textObject.AddComponent<RectTransform>();

                UpdateTextObjectPositionAndAnchors(textObject);
            }
            else
            {
                textObject = text.gameObject;
            }

            return textObject;
        }

        private void InitHighlightTextVariables()
        {
            _highlightTextObject = CreateTextObject("Text_Highlight");
            HighlightText = InitAditionalText(_highlightTextObject, "Text_Highlight");
        }

        private void InitShadowTextVariables()
        {
            _shadowTextObject = CreateTextObject("Text_Shadow");
            ShadowText = InitAditionalText(_shadowTextObject, "Text_Shadow");
        }

        private void UpdateTextObjectPositionAndAnchors(GameObject textObject)
        {
            var shadowRectTransform = _selfObject.GetComponent<RectTransform>();
            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, shadowRectTransform.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, shadowRectTransform.rect.height);
        }

        private GameObject GetSelfObject() => this.gameObject;

        private void SetInitState(bool state) => _initialized = state;

        public void UpdateTextAndShadowValue(string value)
        {
            if (MainText != null)
            {
                MainText.text = value;
            }

            if (ShadowText != null)
            {
                ShadowText.text = value;
            }

            if (HighlightText != null)
            {
                HighlightText.text = value;
            }
        }

        public void UpdateSpriteAsset(TMP_SpriteAsset asset)
        {
            if (MainText != null)
            {
                MainText.spriteAsset = asset;
            }

            if (ShadowText != null)
            {
                ShadowText.spriteAsset = asset;
            }

            if (HighlightText != null)
            {
                HighlightText.spriteAsset = asset;
            }
        }
    }
}