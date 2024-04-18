using System;
using BigCity.Scripts.UI.Base;
using UnityEngine;

namespace Game.UI.ResultScreen
{
    public class ResultScreenView : UiView
    {

        public Action OnOkButtonPressed;
        
        [SerializeField] private GameObject _winLayer;
        [SerializeField] private GameObject _loseLayer;

        public override void Show()
        {
            base.Show();
            var model = (ResultModel)_model;
            _winLayer.SetActive(model.Win);
            _loseLayer.SetActive(!model.Win);
        }

        public void PressOkButton()
        {
            OnOkButtonPressed?.Invoke();
        }
    }
}