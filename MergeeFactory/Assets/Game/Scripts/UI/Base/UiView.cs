using UnityEngine;

namespace BigCity.Scripts.UI.Base
{
    public abstract class UiView : MonoBehaviour, IUiView
    {
        protected IUiModel _model;
        public void SetModel(IUiModel model)
        {
            _model = model;
        }
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetSiblingIndex(int index)
        {
            transform.SetSiblingIndex(index);
        }
    }
}
