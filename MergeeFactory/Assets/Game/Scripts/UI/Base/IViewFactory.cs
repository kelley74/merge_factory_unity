using System;
using BigCity.Scripts.UI.Base;

namespace Game.UI.Base
{
    public interface IViewFactory
    {
        void CreateView(string viewPrefab, Action<IUiView> callback);
    }
}