using System;
using BigCity.Scripts.UI.Base;
using Game.Services;
using UnityEngine;
using Zenject;

namespace Game.UI.Base
{
    public class ViewFactory : IViewFactory
    {
        [Inject] private DiContainer _container;
        [Inject] private IAssetLoader _loader;
        [Inject] private CoroutineManager _coroutineManager;
        [Inject] protected Canvas _mainCanvas;

        public void CreateView(string viewPrefab, Action<IUiView> callback)
        {
            _coroutineManager.StartCoroutine(
                _loader.LoadAndInstantiateAsset(viewPrefab, (asset) =>
                {
                    var view = asset.GetComponent<UiView>();
                    var viewTransform = view.transform;
                    viewTransform.SetParent(_mainCanvas.transform);
                    viewTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    viewTransform.localScale = Vector3.one;

                    var rectTransform = view.GetComponent<RectTransform>();
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                    callback?.Invoke(view);
                }, _container));
        }
    }
}