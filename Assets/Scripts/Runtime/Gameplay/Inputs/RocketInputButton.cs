using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketInputButton : InputButton
    {
        [SerializeField] private TMP_Text _ammoText;
        private IReadOnlyReactiveProperty<int> _ammoCount;
        private IReadOnlyReactiveProperty<int> _maxCount;

        public void Initialize(
            IReadOnlyReactiveProperty<float> reloadProgress,
            IReadOnlyReactiveProperty<int> ammoCount,
            IReadOnlyReactiveProperty<int> maxCount,
            Action onButtonClick)
        {
            base.Initialize(reloadProgress, onButtonClick);
            _ammoCount = ammoCount;
            _maxCount = maxCount;
            _ammoCount.Subscribe(UpdateAmmoVisual).AddTo(_disposables);
            _maxCount.Subscribe(UpdateAmmoVisual).AddTo(_disposables);
        }

        private void UpdateAmmoVisual(int count)
        {
            _ammoText.text = $"{_ammoCount}/{_maxCount}";
        }
    }
}

