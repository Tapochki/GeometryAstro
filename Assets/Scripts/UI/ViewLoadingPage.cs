using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.UI.Views.Base;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public class ViewLoadingPage : View
    {
        private GameObject _wall0;
        private GameObject _wall1;
        private GameObject _wall2;
        private GameObject _wall3;

        private Sequence _sequenceFirstPairOfWall;
        private Sequence _sequenceSecondPairOfWall;
        private Sequence _sequenceLoadingFill;

        private SceneSystem _sceneSystems;

        private float _wall0InitialPositionByX;
        private float _wall1InitialPositionByX;
        private float _wall2InitialPositionByX;
        private float _wall3InitialPositionByX;

        [Inject]
        public void Construct(SceneSystem sceneSystems)
        {
            _sceneSystems = sceneSystems;
        }

        public override void Initialize()
        {
            _wall0 = transform.Find("Container_Walls/Image_Wall_0").gameObject;
            _wall1 = transform.Find("Container_Walls/Image_Wall_1").gameObject;
            _wall2 = transform.Find("Container_Walls/Image_Wall_2").gameObject;
            _wall3 = transform.Find("Container_Walls/Image_Wall_3").gameObject;

            base.Initialize();

            _sceneSystems.LoadAimedAfterLoadingScene();
        }

        public override void Show()
        {
            base.Show();

            CalculateInitialWallPosition();

            InitSequences();

            InitFirstWallsPosition();

            AnimateOpenWalls();

            AnimateCloseWalls();
        }

        private void CalculateInitialWallPosition()
        {
            _wall0InitialPositionByX = _wall0.transform.localPosition.x;//-Screen.width + 500.0f;
            _wall1InitialPositionByX = _wall1.transform.localPosition.x;//Screen.width - 500.0f;
            _wall2InitialPositionByX = _wall2.transform.localPosition.x;//-Screen.width + 300.0f;
            _wall3InitialPositionByX = _wall3.transform.localPosition.x;//Screen.width - 300.0f;
        }

        private void AnimateCloseWalls()
        {
            _sequenceSecondPairOfWall.AppendInterval(2.4f);
            _sequenceSecondPairOfWall.Append(_wall0.transform.DOLocalMove(new Vector3(_wall0InitialPositionByX, 0.0f, 0.0f), 1.0f)).
                                     Join(_wall1.transform.DOLocalMove(new Vector3(_wall1InitialPositionByX, 0.0f, 0.0f), 1.0f));

            _sequenceFirstPairOfWall.AppendInterval(2.8f);
            _sequenceFirstPairOfWall.Append(_wall2.transform.DOLocalMove(new Vector3(_wall2InitialPositionByX, 0.0f, 0.0f), 1.0f)).
                                      Join(_wall3.transform.DOLocalMove(new Vector3(_wall3InitialPositionByX, 0.0f, 0.0f), 1.0f)).
                                      OnComplete(() => _sceneSystems.OpenLoadedScene());
        }

        private void AnimateOpenWalls()
        {
            _sequenceFirstPairOfWall.AppendInterval(0.4f);
            _sequenceFirstPairOfWall.Append(_wall2.transform.DOLocalMove(new Vector3(_wall2InitialPositionByX * 3, 0.0f, 0.0f), 1.0f)).
                                      Join(_wall3.transform.DOLocalMove(new Vector3(_wall3InitialPositionByX * 3, 0.0f, 0.0f), 1.0f));

            _sequenceSecondPairOfWall.AppendInterval(0.6f);
            _sequenceSecondPairOfWall.Append(_wall0.transform.DOLocalMove(new Vector3(_wall0InitialPositionByX * 3, 0.0f, 0.0f), 1.0f)).
                                     Join(_wall1.transform.DOLocalMove(new Vector3(_wall1InitialPositionByX * 3, 0.0f, 0.0f), 1.0f));
        }

        private void InitFirstWallsPosition()
        {
            _wall0.transform.localPosition = new Vector3(_wall0InitialPositionByX, 0.0f, 0.0f);
            _wall1.transform.localPosition = new Vector3(_wall1InitialPositionByX, 0.0f, 0.0f);
            _wall2.transform.localPosition = new Vector3(_wall2InitialPositionByX, 0.0f, 0.0f);
            _wall3.transform.localPosition = new Vector3(_wall3InitialPositionByX, 0.0f, 0.0f);
        }

        private void InitSequences()
        {
            _sequenceFirstPairOfWall = DOTween.Sequence();
            _sequenceSecondPairOfWall = DOTween.Sequence();
            _sequenceLoadingFill = DOTween.Sequence();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _sequenceFirstPairOfWall.Kill();
            _sequenceFirstPairOfWall = null;

            _sequenceSecondPairOfWall.Kill();
            _sequenceSecondPairOfWall = null;

            _sequenceLoadingFill.Kill();
            _sequenceLoadingFill = null;

            _sceneSystems = null;
        }
    }
}