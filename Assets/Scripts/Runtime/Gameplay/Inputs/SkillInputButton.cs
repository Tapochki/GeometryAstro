
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.Gameplay
{
    public class SkillInputButton : InputButton
    {
        [SerializeField] private Image _activeEffectImage;

        public void Initialize(
            IReadOnlyReactiveProperty<float> reloadProgress,
            IReadOnlyReactiveProperty<float> activeSkillProgress,
            Action onButtonClick)
        {
            base.Initialize(reloadProgress, onButtonClick);
            activeSkillProgress.Subscribe(UpdateActiveVisual).AddTo(_disposables);
        }

        private void UpdateActiveVisual(float progress)
        {
            _activeEffectImage.fillAmount = progress;
        }
    }
}

