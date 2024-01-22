using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Studio.Utilities
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

        private bool _initialized;
        private bool _shadowInitialized;

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

        #endregion Main Text

        #region Shadow Text

        [HideInInspector] public Color ShadowColor = Color.black;
        [HideInInspector] public float ShadowThsickness = 0.5f;
        [HideInInspector] public Vector3 ShadowOffset;

        #endregion Shadow Text

        #endregion Text Settings

        public bool GetInitState() => _initialized;

        public bool GetShadowInitState() => _shadowInitialized;

        public bool GetIsHaveShadow() => _isHaveShadow;

        public void SetIsHaveShadow(bool value) => _isHaveShadow = value;

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
            TMP_Text text = ShadowText;
            //ShadowText.color = ShadowColor;
            //ShadowText.raycastTarget = false;

            text.color = ShadowColor;
            text.raycastTarget = false;
            text.outlineWidth = ShadowThsickness;
            text.outlineColor = ShadowColor;
            //ShadowText.outlineWidth = ShadowThsickness;
            //ShadowText.outlineColor = ShadowColor;
            ShadowText = text as TextMeshProUGUI;

            text = null;
            _shadowTextObject.transform.localPosition = ShadowOffset;

            _shadowInitialized = true;
        }

        public bool IsMainTextExist() => GetSelfObject().transform.Find("Text_Main")?.gameObject != null;

        public bool IsShadowTextExist() => GetSelfObject().transform.Find("Text_Shadow")?.gameObject != null;

        #endregion Main Text

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

            MainText = _mainTextObject.GetComponent<TextMeshProUGUI>();
            ShadowText = _shadowTextObject.GetComponent<TextMeshProUGUI>();
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

        private void InitShadowTextVariables()
        {
            _selfObject = null;
            _selfObject = GetSelfObject();

            Transform textObject = _selfObject.transform.Find("Text_Shadow");
            if (textObject == null)
            {
                _shadowTextObject = new GameObject("Text_Shadow");
                _shadowTextObject.transform.parent = _selfObject.transform;
                _shadowTextObject.transform.localScale = Vector3.one;
                _shadowTextObject.transform.localPosition = Vector3.zero;

                _shadowTextObject.transform.SetAsFirstSibling();

                _shadowTextObject.AddComponent<RectTransform>();

                UpdateTextObjectPositionAndAnchors(_shadowTextObject);
            }
            else
            {
                _shadowTextObject = textObject.gameObject;
            }

            if (_shadowTextObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                ShadowText = text;
            }
            else
            {
                ShadowText = _shadowTextObject.AddComponent<TextMeshProUGUI>();
            }
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
        }
    }
}