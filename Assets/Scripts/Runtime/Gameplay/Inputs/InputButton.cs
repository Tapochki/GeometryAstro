using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class InputButton : MonoBehaviour
    {
        [SerializeField] protected Button _button;
        [SerializeField] protected Image _progressImage;

        protected CompositeDisposable _disposables = new CompositeDisposable();
        protected IReadOnlyReactiveProperty<float> _progress;

        public virtual void Initialize(
            IReadOnlyReactiveProperty<float> progress,
            Action onButtonClick)
        {
            _progress = progress;

            _progress.Subscribe(UpdateProgressVisual).AddTo(_disposables);

            _progressImage.fillAmount = 0;

            _button.OnClickAsObservable()
                .Subscribe(_ => onButtonClick?.Invoke())
                .AddTo(_disposables);
        }

        protected virtual void UpdateProgressVisual(float progress)
        {
            _progressImage.fillAmount = 1 - progress;
            SetInteractable((progress >= 1));
        }

        public virtual void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }

        protected virtual void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}

