using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.UI.Views.Base;
using ChebDoorStudio.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public class ViewSplashPage : View
    {
        private Image _iconImage;
        private GameObject _titleText;

        private Sequence _sequence;

        private SceneSystem _sceneSystem;

        [Inject]
        public void Construct(SceneSystem sceneSystem)
        {
            _sceneSystem = sceneSystem;
        }

        public override void Initialize()
        {
            _iconImage = transform.Find("Image_Icon").GetComponent<Image>();
            _titleText = transform.Find("ShadowedText_Title").gameObject;

            base.Initialize();

            _sequence = DOTween.Sequence();
        }

        public override void Show()
        {
            base.Show();

            _sequence = DOTween.Sequence();

            _iconImage.transform.localEulerAngles = new Vector3(90.0f, 0.0f, -45.0f);
            _titleText.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

            _sequence.Append(_iconImage.transform.DORotate(new Vector3(0.0f, 0.0f, -45.0f), 0.5f));
            _sequence.AppendInterval(0.2f);
            _sequence.Append(_iconImage.transform.DOScale(1.2f, 0.6f));
            _sequence.AppendInterval(0.2f);
            _sequence.Append(_iconImage.transform.DORotate(new Vector3(0.0f, 0.0f, -1035.0f), 2.0f)).
                      Join(_titleText.transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 1.0f));
            _sequence.AppendInterval(1.5f);
            _sequence.Append(_iconImage.transform.DOScale(1.0f, 0.6f));
            _sequence.Append(_iconImage.transform.DORotate(new Vector3(90.0f, 0.0f, -1035.0f), 0.3f)).
                      Join(_titleText.transform.DORotate(new Vector3(90.0f, 0.0f, 0.0f), 0.3f));
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