using DG.Tweening;
using TandC.GeometryAstro.Utilities;
using TMPro;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreCountText;

        private int _currentScore;

        public void Init()
        {
            _currentScore = 0;
            UpdateText(0);
        }

        public void UpdateText(int newScore)
        {
            //DOTween.To(() => _currentScore, x =>
            //{
            //    _currentScore = x;
            //    _scoreCountText.text = _currentScore.ToString();
            //}, newScore, 0.5f)
            //.SetEase(Ease.OutQuad);

            InternalTools.DOTextInt(_scoreCountText, _currentScore, newScore, 0.5f);
        }
    }
}

