using Game.UI.Base;
using Game.UI.Gameplay;
using Game.UI.LoadingScreen;
using Game.UI.MainScreen;
using Game.UI.ResultScreen;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private LoadingScreenController _loadingScreen;
        
        public override void InstallBindings()
        {
            Container.Bind<Canvas>().FromInstance(_mainCanvas).AsSingle().NonLazy();
            Container.Bind<IViewFactory>().To<ViewFactory>().FromNew().AsSingle().NonLazy();
            Container.Bind<SiblingSorter>().FromNew().AsSingle().NonLazy();

            Container.Bind<GameplayScreenController>().FromNew().AsSingle().NonLazy();
            Container.Bind<MainScreenController>().FromNew().AsSingle().NonLazy();
            Container.Bind<ResultScreenController>().FromNew().AsSingle().NonLazy();
            Container.Bind<LoadingScreenController>().FromInstance(_loadingScreen).AsSingle().NonLazy();
        }
    }
}
