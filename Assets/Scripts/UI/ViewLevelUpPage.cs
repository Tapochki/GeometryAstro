using Newtonsoft.Json.Linq;
using System;
using TandC.ProjectSystems;
using TandC.Settings;
using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewLevelUpPage : View
    {
        private Button _confirmButton;
        private Button _resetSkillsButton;

        private ShadowedTextMexhProUGUI _infoText;

        private LocalisationSystem _localisationSystem;

        [Inject]
        public void Construct(LocalisationSystem localisationSystem)
        {
            _localisationSystem = localisationSystem;
        }

        public override void Initialize()
        {
            _confirmButton = transform.Find("Image_Background/Container_Buttons/Button_Confirm").GetComponent<Button>();
            _resetSkillsButton = transform.Find("Image_Background/Container_Buttons/Button_ResetSkill").GetComponent<Button>();

            _infoText = transform.Find("Image_Background/ShadowedText_Info").GetComponent<ShadowedTextMexhProUGUI>();

            base.Initialize();

            _confirmButton.onClick.AddListener(ConfirmButtonOnClickHandler);
            _resetSkillsButton.onClick.AddListener(ResetSkillsButtonOnClickHandler);
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
            _resetSkillsButton.onClick.RemoveListener(ResetSkillsButtonOnClickHandler);

            _confirmButton = null;
            _resetSkillsButton = null;
            _infoText = null;
            _localisationSystem = null;
        }

        public override void Update()
        {
            base.Update();
        }

        public void UpdateInfoText(int level)
        {
            _infoText.UpdateTextAndShadowValue(string.Format(_localisationSystem.GetString($"key_levelup_info"), level));
        }

        private void ResetSkillsButtonOnClickHandler()
        {
            TandC.Utilities.Logger.NotImplementedLog("ResetSkillsButton in LevelUpPage"); // TODO - add showing ads and reset skills for choosing
        }

        private void ConfirmButtonOnClickHandler()
        {
            TandC.Utilities.Logger.NotImplementedLog("ConfirmButton in LevelUpPage"); // TODO - add confirmation of choosed skill
        }

        // TODO - add logic for spawning skills for choosing
    }
}