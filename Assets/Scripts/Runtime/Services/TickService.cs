using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Services
{
    public class TickService : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly List<Action> _updateActions = new List<Action>();
        private readonly List<Action> _fixedUpdateActions = new List<Action>();
        private readonly List<Action> _lateUpdateActions = new List<Action>();

        [Inject]
        private IPauseService _pauseService;

        public TickService()
        {
            StartTick();
            StartFixedTick();
            //StartLateTick();
        }

        private void StartTick()
        {
            Observable.EveryUpdate().Where(_ => _pauseService?.IsPaused == false)
                .Subscribe(_ => Tick())
                .AddTo(_disposables);
        }

        private void StartFixedTick()
        {
            Observable.EveryFixedUpdate().Where(_ => _pauseService?.IsPaused == false)
                .Subscribe(_ => FixedTick())
                .AddTo(_disposables);
        }

        private void StartLateTick()
        {
            Observable.EveryLateUpdate().Where(_ => _pauseService != null && !_pauseService.IsPaused)
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

        public void UnregisterUpdate(Action onUpdate)
        {
            _updateActions.Remove(onUpdate);
        }

        public void UnregisterFixedUpdate(Action onFixedUpdate)
        {
            _fixedUpdateActions.Remove(onFixedUpdate);
        }

        public void UnregisterLateUpdate(Action onLateUpdate)
        {
            _lateUpdateActions.Remove(onLateUpdate);
        }

        public IDisposable RegisterTimer(TimeSpan interval, Action callback, bool checkPause = true)
        {
            float timer = 0f;
            return RegisterUpdate(() =>
            {
                if (checkPause && _pauseService?.IsPaused == true) return;

                timer += Time.deltaTime;
                if (timer >= interval.TotalSeconds)
                {
                    timer = 0f;
                    callback?.Invoke();
                }
            });
        }

        public IDisposable RegisterInterval(TimeSpan interval, Action callback, bool checkPause = true)
        {
            float timer = 0f;
            return RegisterUpdate(() =>
            {
                if (checkPause && _pauseService?.IsPaused == true) return;

                timer += Time.deltaTime;
                while (timer >= interval.TotalSeconds)
                {
                    timer -= (float)interval.TotalSeconds;
                    callback?.Invoke();
                }
            });
        }

        private void Tick()
        {
            var actions = _updateActions.ToArray();
            foreach (var action in actions)
            {
                action?.Invoke();
            }
        }

        private void FixedTick()
        {
            var actions = _fixedUpdateActions.ToArray();
            foreach (var action in actions)
            {
                action?.Invoke();
            }
        }

        private void LateTick()
        {
            if (_pauseService.IsPaused == true)
                return;

            var actions = _lateUpdateActions.ToArray();
            foreach (var action in actions)
            {
                action?.Invoke();
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
