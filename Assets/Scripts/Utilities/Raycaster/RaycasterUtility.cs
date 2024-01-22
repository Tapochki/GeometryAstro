using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.Utilities
{
    public class RaycasterUtility : MonoBehaviour
    {
        private GameObject _selfObject;

        private Component[] _images,
                            _textMeshProUguis;

        [SerializeField] private Component[] _whiteList;

        #region Editor Methods

        public void StartRaycastingObject()
        {
            _selfObject = this.gameObject;

            FoundAllImages();
            FoundAllTextMeshProUguis();

            SetOffAllRaycast();
        }

        #endregion Editor Methods

        private void FoundAllImages()
        {
            Debug.Log("Found all images");
            _images = _selfObject.transform.GetComponentsInChildren<Image>();
        }

        private void FoundAllTextMeshProUguis()
        {
            Debug.Log("Found all textMeshProUgui");
            _textMeshProUguis = _selfObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        }

        private void SetOffAllRaycast()
        {
            Debug.Log("Set off all raycast");
            for (int i = 0; i < _images.Length; i++)
            {
                Image image = (Image)_images[i];
                image.raycastTarget = false;
            }

            for (int i = 0; i < _textMeshProUguis.Length; i++)
            {
                TextMeshProUGUI text = (TextMeshProUGUI)_textMeshProUguis[i];
                text.raycastTarget = false;
            }

            Debug.Log("Turn on raycast in white list components");
            foreach (var item in _whiteList)
            {
                if (item is Image)
                {
                    Image image = (Image)item;
                    image.raycastTarget = true;
                }

                if (item is TextMeshProUGUI)
                {
                    TextMeshProUGUI text = (TextMeshProUGUI)item;
                    text.raycastTarget = true;
                }
            }
            Debug.Log("Complete!!!");
        }
    }
}