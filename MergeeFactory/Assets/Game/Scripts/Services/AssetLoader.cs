using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Game.Services
{
    public interface IAssetLoader
    {
        public IEnumerator LoadAndInstantiateAsset(string name, Action<GameObject> onComplete, DiContainer container);
        public IEnumerator LoadAsset<T>(string name, Action<T> onComplete, DiContainer container);
    }
    
    public class AssetLoader : IAssetLoader
    {
        public IEnumerator LoadAndInstantiateAsset(string name, Action<GameObject> onComplete, DiContainer container)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(name);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var asset = container.InstantiatePrefab(handle.Result);
                onComplete?.Invoke(asset);
            }
        }

        public IEnumerator LoadAsset<T>(string name, Action<T> onComplete, DiContainer container)
        {
            var handle = Addressables.LoadAssetAsync<T>(name);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                onComplete?.Invoke(handle.Result);
            }
        }
    }
}
