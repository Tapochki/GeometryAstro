using System;
using TandC.GeometryAstro.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI.Elements
{
    public class SkillItem
    {
        public event Action<PreparationSkillData> ItemSelectionChangedEvent;
        public event Action<PreparationSkillData> ItemConfirmSelectionEvent;
        public bool IsSelect { get; private set; }
        public PreparationSkillData SkillData { get; private set; }

        private GameObject _selfObject;

        private Button _selectButton;
        private Button _buttonConfirmSelection;

        private Color SELECTION_COLOR = new Color(0.0f, 0.4f, 0.6f, 1f);
        private Color DESELECTION_COLOR = new Color(0.0f, 0.32f, 0.32f, 1f);

        public SkillItem(GameObject prefab, bool isChestSkill)
        {
            _selfObject = prefab;
            _selfObject.SetActive(false);
            _selectButton = _selfObject.GetComponent<Button>();
            _buttonConfirmSelection = _selectButton.transform.Find("Button_Confirm").GetComponent<Button>();
            _buttonConfirmSelection.gameObject.SetActive(false);

            SetSelection(false);

            _selectButton.onClick.AddListener(SelectButtonOnClickHandler);
            _buttonConfirmSelection.onClick.AddListener(ConfirmSelectionButtonOnClickHandler);

            if (isChestSkill)
            {
                _selectButton.interactable = false;
            }

            Deselect();
        }

        public void UpdateData(bool isNew, PreparationSkillData preparationSkillData, object[] texts)
        {
            _selfObject.SetActive(true);
            SkillData = preparationSkillData;
            var info = SkillData.SkillUpgradeInfo;

            _selfObject.transform.Find("Image_IconBackground/Image_Icon").GetComponent<Image>().sprite = SkillData.SkillSprite;

            _selfObject.transform.Find("Text_SkillDescription").GetComponent<TextMeshProUGUI>().text = (string)texts[0];
            _selfObject.transform.Find("Text_SkillName").GetComponent<TextMeshProUGUI>().text = (string)texts[1];
            _selfObject.transform.Find("Text_NewMark").gameObject.SetActive(isNew);

            _buttonConfirmSelection.gameObject.SetActive(false);
            SetSelection(false);
            Deselect();
        }

        private void SelectButtonOnClickHandler()
        {
            SetSelection(!IsSelect);
        }

        private void SetSelection(bool state)
        {
            if (IsSelect == state)
            {
                return;
            }

            ItemSelectionChangedEvent?.Invoke(SkillData);
            IsSelect = state;
            _selfObject.GetComponent<Image>().color = SELECTION_COLOR;
            _buttonConfirmSelection.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            _selfObject.GetComponent<Image>().color = DESELECTION_COLOR;
            IsSelect = false;
            _buttonConfirmSelection.gameObject.SetActive(false);
        }

        private void ConfirmSelectionButtonOnClickHandler()
        {
            ItemConfirmSelectionEvent?.Invoke(SkillData);
        }

        public void Hide()
        {
            _selfObject.SetActive(false);
        }

        public bool IsActive() => _selectButton.IsActive();
    }
}