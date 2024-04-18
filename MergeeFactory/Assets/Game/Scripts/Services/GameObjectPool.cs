using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public class GameObjectPool
    {
        private readonly Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
        private Action<string, Action<GameObject>> _missingObjectHandler;

        public void Init(Action<string, Action<GameObject>> missingObjectHandler)
        {
            _missingObjectHandler = missingObjectHandler;
        }
        
        public void GetGameObject(string id, Action<GameObject> onComplete)
        {
            if (_pool.TryGetValue(id, out var concretePool))
            {
                if (concretePool.Count > 0)
                {
                    var obj = concretePool.Dequeue();
                    onComplete.Invoke(obj);
                    return;
                }
            }
            else
            {
                _pool.Add(id, new Queue<GameObject>());
            }
            _missingObjectHandler.Invoke(id, onComplete);
        }

        public void ReturnObject(string id, GameObject gameObject)
        {
            if (_pool.TryGetValue(id, out var value))
            {
                value.Enqueue(gameObject);
            }
            else
            {
                var concretePool = new Queue<GameObject>();
                concretePool.Enqueue(gameObject);
                _pool.Add(id,concretePool);
            }
        }
    }
}