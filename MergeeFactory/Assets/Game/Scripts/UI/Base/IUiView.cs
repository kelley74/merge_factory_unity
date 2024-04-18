namespace BigCity.Scripts.UI.Base
{
    public interface IUiView
    {
        void SetModel(IUiModel model);
        void Show();
        void Hide();
        
        void SetSiblingIndex(int index);
    }
}