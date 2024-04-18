using Game.Core;
using Game.UI.Base;
using Game.UI.LoadingScreen;
using Zenject;

namespace Game.UI.MainScreen
{
    public class MainScreenController : UiController<MainScreenView>
    {
        protected override string ViewPrefab => "MainScreen";
        protected override int SiblingIndex => 111;

        [Inject] private LoadingScreenController _loadingScreen;
        [Inject] private GameMaster _gameMaster;

        private bool _lock;
        
        protected override void OnOpen()
        {
            _view.OnStartPressed += StartNewGame;
            _lock = false;
        }
        
        protected override void OnClose()
        {
            _view.OnStartPressed -= StartNewGame;
        }

        private void StartNewGame()
        {
            if (_lock)
            {
                return;
            }
            _lock = true;
            _loadingScreen.Close(() =>
            {
                Close();
                _gameMaster.CreateNewRound();
            });
        }
    }
}
