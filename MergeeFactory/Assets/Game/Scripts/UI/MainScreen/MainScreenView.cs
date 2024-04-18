using System;
using BigCity.Scripts.UI.Base;

namespace Game.UI.MainScreen
{
    public class MainScreenView : UiView
    {
        public Action OnStartPressed;
        
        public void PressStartButton()
        {
            OnStartPressed?.Invoke();
        }
    }
}
