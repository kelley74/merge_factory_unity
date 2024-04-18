using UnityEngine;

namespace BigCity.Scripts.UI.Base
{
    public class ViewProvider : MonoBehaviour
    {
        [SerializeField] private UiView[] _views;

        public UiView GetView(string viewName)
        {
            foreach (var view in _views)
            {
                if (view.name == viewName)
                {
                    return view;
                }
            }

            return null;
        }
    }
}
