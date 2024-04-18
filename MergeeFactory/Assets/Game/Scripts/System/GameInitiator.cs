using System.Collections;
using Game.UI.LoadingScreen;
using Game.UI.MainScreen;
using UnityEngine;
using Zenject;

namespace Game.System
{
    public class GameInitiator : MonoBehaviour
    {
        [Inject] private MainScreenController _mainScreen;
        [Inject] private LoadingScreenController _loadingScreen;
        private IEnumerator Start()
        {
            _loadingScreen.Close(null,true);
            Application.targetFrameRate = 60;
            yield return new WaitForSeconds(0.3f);
            yield return _mainScreen.Open();
            _loadingScreen.Open();
        }
    }
}
