using System;

namespace TandC.GeometryAstro.UI
{
    public interface IUIPopup
    {
        bool IsActive { get; }

        void Init();
        void Dispose();
        void Show(object data = null);
        void Hide();
    }
}