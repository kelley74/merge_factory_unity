using System.Collections;
using Game.Services;
using Game.UI.Base;
using Game.UI.LoadingScreen;
using Game.UI.MainScreen;
using Zenject;

namespace Game.UI.ResultScreen
{
    public class ResultScreenController : UiController<ResultScreenView>
    {
        protected override string ViewPrefab => "ResultScreen";
        protected override int SiblingIndex => 241;

        [Inject] private MainScreenController _mainScreen;
        [Inject] private CoroutineManager _coroutineManager;
        [Inject] private LoadingScreenController _loadingScreen;
        
        protected override void OnOpen()
        {
            _view.OnOkButtonPressed += OnOkButtonPressed;
        }
        
        protected override void OnClose()
        {
            _view.OnOkButtonPressed -= OnOkButtonPressed;
        }
        
        private void OnOkButtonPressed()
        {
            _coroutineManager.StartCoroutine(GoToMainMenu());
        }

        private IEnumerator GoToMainMenu()
        {
            _loadingScreen.Close(null);
            yield return 0.5f;
            yield return _mainScreen.Open();
            Close();
            _loadingScreen.Open();
        }
    }
}
