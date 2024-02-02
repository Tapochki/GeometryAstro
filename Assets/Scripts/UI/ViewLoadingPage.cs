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

        [Inject]
        public void Construct(SceneSystem sceneSystems)
        {
            _sceneSystems = sceneSystems;
        }

        public override void Initialize()
        {
            base.Initialize();

            _sceneSystems.LoadAimedAfterLoadingScene();
        }

        public override void Show()
        {
            base.Show();

            InternalTools.DoActionDelayed(() => _sceneSystems.OpenLoadedScene(), 2.0f);
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