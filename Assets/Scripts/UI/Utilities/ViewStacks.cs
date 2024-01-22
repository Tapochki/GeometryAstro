using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.UI.Views;
using ChebDoorStudio.UI.Views.Base;
using System.Collections.Generic;

namespace ChebDoorStudio.UI
{
    public class ViewStacks
    {
        private UISystem _uiSystem;
        private InputsSystem _inputsSystem;
        private GameStateSystem _gameStateSystem;

        private View _currentView;

        private Stack<View> _stackOfViews;

        public ViewStacks(UISystem uiSystem, InputsSystem inputsSystem, GameStateSystem gameStateSystem)
        {
            _uiSystem = uiSystem;
            _inputsSystem = inputsSystem;
            _gameStateSystem = gameStateSystem;

            _stackOfViews = new Stack<View>();

            _inputsSystem.OnEscapeButtonDownEvent += OnEscapeButtonDownEventHandler;
        }

        private void OnEscapeButtonDownEventHandler()
        {
            if (_stackOfViews.Count <= 0)
            {
                if (_gameStateSystem.GameStarted)
                {
                    _uiSystem.ShowView<ViewPausePage>();
                    return;
                }
                else
                {
                    _uiSystem.ShowView<ViewExitPage>();
                    return;
                }
            }

            _uiSystem.HideCurrentView();
        }

        public void AddViewToTopOfStack(View panel)
        {
            _currentView = panel;
            _stackOfViews.Push(panel);
        }

        public void RemoveViewFromTopOfStack()
        {
            if (_stackOfViews.Count > 0)
            {
                _stackOfViews.Pop();
            }

            _currentView = GetFirstViewInStack();
        }

        public View GetFirstViewInStack()
        {
            if (_stackOfViews.Count > 0)
            {
                return _stackOfViews.Peek();
            }

            return null;
        }

        public void Dispose()
        {
            _inputsSystem.OnEscapeButtonDownEvent -= OnEscapeButtonDownEventHandler;

            _currentView = null;

            _stackOfViews.Clear();
            _stackOfViews = null;

            _uiSystem = null;
            _inputsSystem = null;
            _gameStateSystem = null;
        }
    }
}