using DG.Tweening;
using TandC.ProjectSystems;
using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewLoadingPage : View
    {
        private SceneSystem _sceneSystems;
        private LocalisationSystem _localisationSystem;

        private ShadowedTextMexhProUGUI _loadingTitleText;

        [Inject]
        public void Construct(SceneSystem sceneSystems, LocalisationSystem localisationSystem)
        {
            _sceneSystems = sceneSystems;
            _localisationSystem = localisationSystem;
        }

        public override void Initialize()
        {
            _loadingTitleText = transform.Find("ShadowedText_Title").GetComponent<ShadowedTextMexhProUGUI>();

            base.Initialize();

            _loadingTitleText.UpdateTextAndShadowValue(_localisationSystem.GetString("key_loading_title"));

            _sceneSystems.LoadAimedAfterLoadingScene();
        }

        public override void Show()
        {
            base.Show();

            InternalTools.DoActionDelayed(() => _sceneSystems.OpenLoadedScene(), 4.0f);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _sceneSystems = null;
        }
    }
}