using ChebDoorStudio.ScriptableObjects;
using ChebDoorStudio.Settings;
using ChebDoorStudio.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ChebDoorStudio.UI.Views.Objects
{
    public class ShopItem : MonoBehaviour
    {
        public event Action<ShopItem> OnItemSelectedEvent;

        public bool IsSelected { get; private set; }
        public ShopItemType Type { get; private set; }
        public int Price { get; private set; }
        public ShopItemData Data { get; private set; }
        public bool IsBoughted { get; private set; }

        private GameObject _containerNotBoughtedObject;

        private Button _selectionButton;
        private Image _iconImage;
        private ShadowedTextMexhProUGUI _priceText;
        private Image _highlightImage;

        public void Initialize(ShopItemData data, bool isBoughted)
        {
            _selectionButton = GetComponent<Button>();

            _iconImage = transform.Find("Image_Icon").GetComponent<Image>();
            _priceText = transform.Find("Container_NotNoughted/ShadowedText_Price").GetComponent<ShadowedTextMexhProUGUI>();
            _highlightImage = transform.Find("Image_Highlight").GetComponent<Image>();

            _containerNotBoughtedObject = transform.Find("Container_NotNoughted").gameObject;

            IsBoughted = isBoughted;

            Data = data;

            _iconImage.sprite = Data.itemPreview;
            Type = Data.type;
            Price = Data.price;

            _priceText.UpdateTextAndShadowValue(Price.ToString());

            SetSelected(false);

            _selectionButton.onClick.AddListener(SelectionButtonOnClickHandler);

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (IsBoughted)
            {
                _containerNotBoughtedObject.SetActive(false);
            }
            else
            {
                _containerNotBoughtedObject.SetActive(true);
            }
        }

        public void SetBoughted()
        {
            IsBoughted = true;

            UpdateStatus();
        }

        private void SelectionButtonOnClickHandler()
        {
            SetSelected(true);

            OnItemSelectedEvent?.Invoke(this);
        }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            _highlightImage.gameObject.SetActive(IsSelected);
        }

        public void DIspose()
        {
        }
    }
}