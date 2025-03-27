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
            InternalTools.DOTextInt(_scoreCountText, _currentScore, newScore, 0.5f);

            _currentScore = newScore;
        }
    }
}

