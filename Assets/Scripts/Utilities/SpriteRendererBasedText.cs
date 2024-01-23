using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TandC.Utilities
{
    [ExecuteAlways]
    public class SpriteRendererBasedText : MonoBehaviour
    {
        [SerializeField]
        private List<CharImage> _images = new List<CharImage>();

        private TweenerCore<int, int, NoOptions> _countingTween;

        public List<SpriteBasedChar> chars;

        public float horizontalOffset;

        public float size = 1f;

        public bool isCentering;

        public ObjectSorting.SortingLayer sortingLayer;

        public uint layer;

        public Color color = Color.white;

        public string text;

        private void Update()
        {
            if (isCentering)
            {
                if (_images.Count > 1)
                {
                    float size = _images[_images.Count - 1].Width + horizontalOffset;
                    float position = -size * (float)(_images.Count / 2f) + (size / 2);
                    for (int i = 0; i < _images.Count; i++)
                    {
                        _images[i].SetPosition(position);
                        position += size;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i].SetPosition(i > 0 ? (_images[i - 1].Position.x + _images[i - 1].Width + horizontalOffset) : 0);
                }
            }
        }

        private void OnEnable()
        {
        }

        public void Clean(bool all = true)
        {
            if (all)
            {
                _countingTween?.Kill();
                _countingTween = null;
            }

            if (_images.Count > 0)
            {
                foreach (var item in _images)
                {
                    item.Dispose();
                }

                _images.Clear();
            }
        }

        public void SetText(int number, bool countAnimation, Action countAnimationCallback = null, float animationDuration = 1.5f, int startFrom = 0, bool fade = false, bool increase = false)
        {
            if (countAnimation)
            {
                if (increase)
                {
                    int.TryParse(text, out startFrom);
                    fade = false;
                }

                int current = startFrom;
                int previous = current;

                SetText(current.ToString(), fade);

                _countingTween?.Kill();
                _countingTween = DOTween.To(() => startFrom, x => current = x, number, animationDuration).SetEase(Ease.Unset).OnUpdate(() =>
                {
                    if (current != previous)
                    {
                        previous = current;

                        SetText(current.ToString());
                    }
                }).OnComplete(() =>
                {
                    _countingTween = null;
                    countAnimationCallback?.Invoke();
                });
            }
            else
            {
                SetText(number.ToString(), fade);
            }
        }

        public void SetText(string text, bool fade = false)
        {
            this.text = text;

            Clean(false);

            if (string.IsNullOrEmpty(text) || !transform)
            {
                return;
            }

            char[] chars = text.ToCharArray();

            CharImage charImage;
            for (int i = 0; i < chars.Length; i++)
            {
                charImage = new CharImage(GetByChar(chars[i]), transform);
                charImage.SetSize(size);
                charImage.SetLayer(sortingLayer, layer);
                charImage.SetColor(color);
                _images.Add(charImage);

                if (fade)
                {
                    charImage.ShowFromAlpha();
                }
            }

            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].SetPosition(i > 0 ? (_images[i - 1].Position.x + _images[i - 1].Width) : 0);
            }
        }

        private SpriteBasedChar GetByChar(char glyph)
        {
            if (chars == null)
            {
                return new SpriteBasedChar()
                {
                    glyph = glyph
                };
            }

            var item = chars.Find(it => it.glyph == glyph);

            if (item == null)
            {
                item = new SpriteBasedChar()
                {
                    glyph = glyph
                };
            }

            return item;
        }

        [Serializable]
        public class SpriteBasedChar
        {
            public char glyph;
            public Sprite sprite;
        }

        [Serializable]
        private class CharImage
        {
            private GameObject _selfObject;

            private SpriteRenderer _image;

            private ObjectSorting _objectSorting;

            private TweenerCore<Color, Color, ColorOptions> _fadeTween;

            public float Width => _image.bounds.size.x;
            public Vector3 Position => _selfObject.transform.localPosition;

            public CharImage(SpriteBasedChar spriteBasedChar, Transform parent)
            {
                _selfObject = new GameObject($"Char_{spriteBasedChar.glyph}");
                _selfObject.layer = parent.gameObject.layer;

                _image = _selfObject.AddComponent<SpriteRenderer>();
                _selfObject.transform.SetParent(parent, false);
                _selfObject.transform.localPosition = Vector3.zero;

                _objectSorting = _selfObject.AddComponent<ObjectSorting>();

                _image.sprite = spriteBasedChar.sprite;
            }

            public void ShowFromAlpha()
            {
                Color color = _image.color;
                color.a = 0;
                _image.color = color;
                _fadeTween = _image.DOFade(1f, 0.1f).OnComplete(() =>
                {
                    _fadeTween = null;
                });
            }

            public void Dispose()
            {
                _fadeTween?.Kill();
                _fadeTween = null;
                MonoBehaviour.DestroyImmediate(_selfObject);
            }

            public void SetSize(float size)
            {
                _selfObject.transform.localScale = Vector3.one * size;
            }

            public void SetLayer(ObjectSorting.SortingLayer layer, uint order)
            {
                _objectSorting.sortingLayer = layer;
                _objectSorting.sortingOrder = order;
            }

            public void SetPosition(float offset)
            {
                var position = _selfObject.transform.localPosition;
                position.x = offset;
                _selfObject.transform.localPosition = position;
            }

            public void SetColor(Color color)
            {
                _image.color = color;
            }
        }
    }
}