using System;
using UnityEngine;
using Zenject;

namespace Game.Services
{
    public class ServiceInstaller : MonoInstaller
    {
        private IAssetLoader _assetLoader;
        private CoroutineManager _coroutineManager;
        [Inject] private DiContainer _container;
        public override void InstallBindings()
        {
            _assetLoader = new AssetLoader();
            _coroutineManager = gameObject.AddComponent<CoroutineManager>();
            Container.Bind<IAssetLoader>().FromInstance(_assetLoader).AsSingle().NonLazy();
            Container.Bind<CoroutineManager>().FromInstance(_coroutineManager).AsSingle()
                .NonLazy();


            var pool = new GameObjectPool();
            pool.Init(CreatePoolObject);

            Container.Bind<GameObjectPool>().FromInstance(pool).AsSingle().NonLazy();
        }

        private void CreatePoolObject(string id, Action<GameObject> onComplete)
        {
            _coroutineManager.StartCoroutine(_assetLoader.LoadAndInstantiateAsset(id, onComplete, _container));
        }
    }
}