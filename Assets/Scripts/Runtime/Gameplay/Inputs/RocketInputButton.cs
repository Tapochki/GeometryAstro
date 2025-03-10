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

        public void Initialize(
            IReadOnlyReactiveProperty<float> reloadProgress,
            IReadOnlyReactiveProperty<int> ammoCount,
            Action onButtonClick)
        {
            base.Initialize(reloadProgress, onButtonClick);
            _ammoCount = ammoCount;

            _ammoCount.Subscribe(UpdateAmmoVisual).AddTo(_disposables);
        }

        private void UpdateAmmoVisual(int count)
        {
            _ammoText.text = $"{count}";
        }
    }
}

