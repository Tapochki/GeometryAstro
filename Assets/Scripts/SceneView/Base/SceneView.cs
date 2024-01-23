using System.Collections.Generic;
using TandC.ProjectSystems;
using TandC.UI.Views.Base;
using UnityEngine;
using Zenject;

namespace TandC.Scenes.Base
{
    public class SceneView : MonoBehaviour
    {
        protected View _rootView;

        protected List<View> _views;

        protected UISystem _uiSystem;

        [Inject]
        public void Construct(UISystem uiSystem)
        {
            _uiSystem = uiSystem;
        }

        public virtual void Initialize()
        {
            if (_rootView != null)
            {
                _views = _uiSystem.TryToRemoveRootViewFromViewList(_views, _rootView);
            }

            _views = _uiSystem.TryToRemoveDuplicatesFromViewList(_views);

            _uiSystem.SetupViewsInCurrentScene(_views, _rootView, this);

            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Initialize();

                _views[i].Hide();
            }

            if (_rootView != null)
            {
                _rootView.Initialize();

                _rootView.Show();
            }
        }

        private void Update()
        {
            if (_views == null)
            {
                return;
            }

            if (_rootView != null)
            {
                _rootView.Update();
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

            _rootView.Dispose();
            _rootView = null;

            _uiSystem = null;
        }
    }
}