using Studio.Settings;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio.Utilities
{
    [RequireComponent(typeof(Image))]
    public class MultiComponentButton : Button, IPointerDownHandler, IPointerUpHandler
    {
        public List<Image> Images = new List<Image>();
        public List<TextMeshProUGUI> Texts = new List<TextMeshProUGUI>();

        public Image icon;

        public string title;

        public void UpdateTitle()
        {
            gameObject.name = "MultiComponentButton_" + title;
        }

        public void AddIconObject()
        {
            icon = new GameObject("Image_Icon").AddComponent<Image>();
            icon.transform.parent = GetSelfObject().transform;
            icon.transform.localScale = Vector3.one;
            icon.transform.localPosition = Vector3.zero;
            icon.raycastTarget = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (IsIconExist())
            {
                UpdateColorImage(0.8f, icon);
            }

            foreach (var image in Images)
            {
                UpdateColorImage(0.8f, image);
            }

            foreach (var text in Texts)
            {
                UpdateColorText(0.8f, text);
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (IsIconExist())
            {
                UpdateColorImage(1.25f, icon);
            }

            foreach (var image in Images)
            {
                UpdateColorImage(1.25f, image);
            }

            foreach (var text in Texts)
            {
                UpdateColorText(1.25f, text);
            }
        }

        private void UpdateColorImage(float multiplayer, Image image) // down = 0.8, up = 1.25
        {
            if (image == null)
            {
                Utilities.Logger.Log("Somewhere image is null. Please check!", LogTypes.Warning);
                return;
            }

            Color color = image.color;
            color *= multiplayer;
            color.a = 1f;
            image.color = color;
        }

        private void UpdateColorText(float multiplayer, TextMeshProUGUI text) // down = 0.8, up = 1.25
        {
            if (text == null)
            {
                Utilities.Logger.Log("Somewhere text is null. Please check!", LogTypes.Warning);
                return;
            }

            Color color = text.color;
            color *= multiplayer;
            color.a = 1f;
            text.outlineColor = color;
            text.color = color;
        }

        public bool IsIconExist() => icon != null;

        private GameObject GetSelfObject() => this.gameObject;
    }
}