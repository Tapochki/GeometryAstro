using System;
using TandC.ProjectSystems;
using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewChestPage : View
    {
        private Button _confirmButton;

        private GameObject _flashLightContainer;
        private GameObject _getRewardContainer;

        private ShadowedTextMexhProUGUI _coinsText;

        private Animator _animator;

        private LocalisationSystem _localisationSystem;

        [Inject]
        public void Construct(LocalisationSystem localisationSystem)
        {
            _localisationSystem = localisationSystem;
        }

        public override void Initialize()
        {
            _confirmButton = transform.Find("Container/Button_Confirm").GetComponent<Button>();

            _coinsText = transform.Find("Container_Coins/Text_Coins").GetComponent<ShadowedTextMexhProUGUI>();

            _animator = _flashLightContainer.GetComponent<Animator>();

            _flashLightContainer = transform.Find("Flashlight").gameObject;
            _getRewardContainer = transform.Find("Container").gameObject;

            base.Initialize();

            _confirmButton.onClick.AddListener(ConfirmButtonOnClickHandler);
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _confirmButton.onClick.RemoveListener(ConfirmButtonOnClickHandler);
        }

        public override void Update()
        {
            base.Update();
        }

        public void UpdateCoinsText(int amount)
        {
            _coinsText.UpdateTextAndShadowValue(amount.ToString());
        }

        private void ConfirmButtonOnClickHandler()
        {
            TandC.Utilities.Logger.NotImplementedLog("ConfirmButton in ChestPage"); // TODO - add confirmation of getted skills
        }
    }
}