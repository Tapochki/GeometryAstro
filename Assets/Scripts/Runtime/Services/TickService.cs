using System;
using System.Collections.Generic;
using UniRx;

namespace TandC.GeometryAstro.Services 
{
    public class TickService : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly List<Action> _updateActions = new List<Action>();
        private readonly List<Action> _fixedUpdateActions = new List<Action>();
        private readonly List<Action> _lateUpdateActions = new List<Action>();

        public TickService()
        {
            StartTick();
            StartFixedTick();
            StartLateTick();
        }

        private void StartTick() 
        {
            Observable.EveryUpdate()
                .Subscribe(_ => Tick())
                .AddTo(_disposables);
        }

        private void StartFixedTick() 
        {
            Observable.EveryFixedUpdate()
                .Subscribe(_ => FixedTick())
                .AddTo(_disposables);
        }

        private void StartLateTick() 
        {
            Observable.EveryLateUpdate()
                .Subscribe(_ => LateTick())
                .AddTo(_disposables);
        }

        public IDisposable RegisterUpdate(Action onUpdate)
        {
            _updateActions.Add(onUpdate);
            return Disposable.Create(() => _updateActions.Remove(onUpdate));
        }

        public IDisposable RegisterFixedUpdate(Action onFixedUpdate)
        {
            _fixedUpdateActions.Add(onFixedUpdate);
            return Disposable.Create(() => _fixedUpdateActions.Remove(onFixedUpdate));
        }

        public IDisposable RegisterLateUpdate(Action onLateUpdate)
        {
            _lateUpdateActions.Add(onLateUpdate);
            return Disposable.Create(() => _lateUpdateActions.Remove(onLateUpdate));
        }

        private void Tick()
        {
            var actions = _updateActions.ToArray();
            foreach (var action in actions) action?.Invoke();
        }

        private void FixedTick()
        {
            var actions = _fixedUpdateActions.ToArray();
            foreach (var action in actions) action?.Invoke();
        }

        private void LateTick()
        {
            var actions = _lateUpdateActions.ToArray();
            foreach (var action in actions) action?.Invoke();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
