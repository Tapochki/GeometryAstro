using DG.Tweening;
using TandC.ProjectSystems;
using TandC.UI.Views.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewSplashPage : View
    {
        private Image _shadeImage;
        private Image _iconImage;

        private Sequence _sequence;

        private SceneSystem _sceneSystem;

        private Color _iconColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        [Inject]
        public void Construct(SceneSystem sceneSystem)
        {
            _sceneSystem = sceneSystem;
        }

        public override void Initialize()
        {
            _shadeImage = transform.Find("Image_ShadedIcon/Image_Shade").GetComponent<Image>();
            _iconImage = transform.Find("Image_Icon").GetComponent<Image>();

            base.Initialize();

            _sequence = DOTween.Sequence();
        }

        public override void Show()
        {
            base.Show();

            _sequence = DOTween.Sequence();

            _iconImage.color = _iconColor;

            _shadeImage.transform.localPosition = new Vector3(-800.0f, 256.0f, 0.0f);

            _sequence.AppendInterval(0.2f);
            _sequence.Append(_iconImage.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), 6.0f));
            _sequence.Join(_shadeImage.transform.DOLocalMove(new Vector3(800.0f, 256.0f, 0.0f), 4.0f));
            _sequence.AppendInterval(0.2f);
            _sequence.OnComplete(() => _sceneSystem.OpenLoadedScene());
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _sequence.Kill();
            _sequence = null;

            _sceneSystem = null;
        }
    }
}