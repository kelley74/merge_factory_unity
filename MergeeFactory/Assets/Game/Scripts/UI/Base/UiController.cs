using System.Collections;
using BigCity.Scripts.UI.Base;
using Zenject;

namespace Game.UI.Base
{
    public interface ISiblingSettable
    {
        void ResetSibling(int i);
        int CurrentSiblingIndex { get; }
    }

    public abstract class UiController<T> : ISiblingSettable where T : IUiView
    {
        [Inject] private SiblingSorter _siblingSorter;
        [Inject] private IViewFactory _viewFactory;

        protected T _view;
        protected IUiModel _model;
        protected abstract string ViewPrefab { get; }
        protected abstract int SiblingIndex { get; }

        public int CurrentSiblingIndex => SiblingIndex;

        private bool _loaded;

        public IEnumerator Open(IUiModel model = null)
        {
            _loaded = false;
            _model = model;

            if (_view == null)
            {
                _viewFactory.CreateView(ViewPrefab, (view) =>
                {
                    _view = (T)view;
                    _view.SetModel(model);
                    _siblingSorter.RegisterUiController(this);
                    _view.Show();
                    OnOpen();
                    _loaded = true;
                });
            }
            else
            {
                _view.SetModel(model);
                _view.Show();
                OnOpen();
                _loaded = true;
            }

            while (!_loaded)
            {
                yield return null;
            }
        }

        public void ResetSibling(int i)
        {
            _view.SetSiblingIndex(i);
        }

        public void Close()
        {
            _view.Hide();
            OnClose();
        }

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
        }
    }
}