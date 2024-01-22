using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.UI.Views.Base;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.Scenes.Base
{
    public class SceneView : MonoBehaviour
    {
        protected View _menuRootView;
        protected View _gameRootView;

        protected List<View> _views;

        protected UISystem _uiSystem;

        [Inject]
        public void Construct(UISystem uiSystem)
        {
            _uiSystem = uiSystem;
        }

        public virtual void Initialize()
        {
            if (_menuRootView != null)
            {
                _views = _uiSystem.TryToRemoveRootViewFromViewList(_views, _menuRootView);
            }

            if (_gameRootView != null)
            {
                _views = _uiSystem.TryToRemoveRootViewFromViewList(_views, _gameRootView);
            }

            _views = _uiSystem.TryToRemoveDuplicatesFromViewList(_views);

            _uiSystem.SetupViewsInCurrentScene(_views, _menuRootView, _gameRootView, this);

            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Initialize();

                _views[i].Hide();
            }

            if (_menuRootView != null)
            {
                _menuRootView.Initialize();

                _menuRootView.Show();
            }

            if (_gameRootView != null)
            {
                _gameRootView.Initialize();

                _gameRootView.Hide();
            }
        }

        public void GameStarted()
        {
            _menuRootView.Hide();
            _gameRootView.Show();
        }

        public void GameStoped()
        {
            _menuRootView.Show();
            _gameRootView.Hide();
        }

        private void Update()
        {
            if (_views == null)
            {
                return;
            }

            if (_menuRootView != null)
            {
                _menuRootView.Update();
            }

            if (_gameRootView != null)
            {
                _gameRootView.Update();
            }

            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Update();
            }
        }

        public virtual void ShowView(View view)
        {
            _uiSystem.ShowView(view);
        }

        public virtual void ShowView<T>() where T : View
        {
            _uiSystem.ShowView<T>();
        }

        public virtual void HideView()
        {
            _uiSystem.HideCurrentView();
        }

        public virtual void Dispose()
        {
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Dispose();
            }

            _views.Clear();

            _views = null;

            _menuRootView.Dispose();
            _menuRootView = null;

            if (_gameRootView != null)
            {
                _gameRootView.Dispose();

                _gameRootView = null;
            }

            _uiSystem = null;
        }
    }
}