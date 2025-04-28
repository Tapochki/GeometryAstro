using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.UI;
using UnityEngine;

namespace TandC.GeometryAstro.Utilities
{
    public class UIService : ILoadUnit
    {
        private List<IUIPage> _pages;
        private List<IUIPopup> _popups;

        public IUIPage CurrentPage { get; private set; }
        public IUIPopup CurrentPopup { get; private set; }
        public GameObject Canvas { get; private set; }

        public async UniTask Load()
        {
            Initialize();
            await UniTask.CompletedTask;
        }

        public void Initialize()
        {
            Canvas = GameObject.Find("Canvas");

            foreach (var page in _pages)
            {
                page.Init();
            }

            foreach (var popup in _popups)
            {
                popup.Init();
            }
        }

        public void RegisterUI(List<IUIPage> pages, List<IUIPopup> popups)
        {
            _pages = pages;
            _popups = popups;
        }

        public void OpenPage<T>(object data = null) where T : IUIPage
        {
            if (CurrentPage != null)
            {
                CurrentPage.Hide();
            }

            HideAllPages();

            foreach (var _page in _pages)
            {
                if (_page is T)
                {
                    CurrentPage = _page;
                    break;
                }
            }
            CurrentPage.Show();
        }

        public void HideAllPages()
        {
            foreach (var _page in _pages)
            {
                _page.Hide();
            }
        }

        public void HideAllPopups()
        {
            foreach (var _popup in _popups)
            {
                _popup.Hide();
            }
        }

        public void ShowPopup<T>(object data = null, bool isMain = false) where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _popups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            if (isMain)
            {
                // TODO
            }
            popup.Show(data);
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in _popups)
            {
                if (_popup is T)
                {
                    _popup.Hide();
                    break;
                }
            }
        }

        public void Dispose()
        {
            foreach (var _page in _pages)
            {
                _page.Dispose();
            }

            foreach (var _popup in _popups)
            {
                _popup.Dispose();
            }
        }
    }
}