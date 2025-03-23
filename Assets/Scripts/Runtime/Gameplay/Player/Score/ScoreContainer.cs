using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ScoreContainer
    {
        public int Score { get; private set; }

        private readonly ScoreView _scoreView;

        public ScoreContainer() 
        {
            _scoreView = GameObject.FindAnyObjectByType<ScoreView>();
            _scoreView.Init();
        }

        public void Init()
        {
            Score = 0;
        }

        public void AddScore(int count, float scoreModificator) 
        {
            Score += Mathf.RoundToInt(count * scoreModificator);
            UpdateView();
        }

        public void UpdateView() 
        {
            _scoreView.UpdateText(Score);
        }
    }
}

