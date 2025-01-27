using System;
using TandC.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.UI.Views 
{
    public class SkillItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _labelText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _spriteIcon;
        [SerializeField] private Image _backGround;
        [SerializeField] private GameObject _newIconImage;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Button _buttonConfirmSelection;

        private Action<SkillItemView> _itemSelectionEvent;
        private Action<SkillItemView> _itemActivateEvent;

        public bool isSelect { get; private set; }

        public bool isChestSkill;

        public void Init() 
        {
            _selectButton.onClick.AddListener(SelectButtonOnClickHandler);
            _buttonConfirmSelection.onClick.AddListener(ConfirmSelectionButtonOnClickHandler);
        }

        public void Show(SkillDescription skillDescription, int level) 
        {
            _labelText.text = skillDescription.name;
            _descriptionText.text = skillDescription.skillDescription;
            if(level == 1) 
            {
                _newIconImage.SetActive(true);
            }
            else 
            {
                _newIconImage.SetActive(false);
            }
            _spriteIcon.sprite = skillDescription.skillIcon;
            _levelText.text = skillDescription.name;
            gameObject.SetActive(true);
        }

        public void Hide() 
        {
            gameObject.SetActive(false);
        }

        private void SelectButtonOnClickHandler()
        {
            SetSelection(!isSelect);
        }

        public void SetSelection(bool state)
        {
            if (isSelect == state)
                return;

            _itemSelectionEvent?.Invoke(this);
            isSelect = state;
            _backGround.color = new Color(0.0f, 0.4f, 0.6f, 1f);
            _buttonConfirmSelection.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            _backGround.color = new Color(0.0f, 0.32f, 0.32f, 1f);
            isSelect = false;
            _buttonConfirmSelection.gameObject.SetActive(false);
        }

        private void ConfirmSelectionButtonOnClickHandler()
        {
            _itemActivateEvent?.Invoke(this);
        }
    }
}


