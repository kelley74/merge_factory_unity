using System;
using System.Collections.Generic;
using System.Linq;
using BigCity.Scripts.UI.Base;
using Game.Configs.LevelData;

namespace Game.UI.Gameplay
{
    public class GameplayModel : IUiModel
    {
        private Dictionary<string, int> _levelTargets;
        private Dictionary<string, ILevelElement> _levelTargetElements;
        private Dictionary<string, Action<int>> _targetUpdateHandlers;
        private ILevelElement[] _levelElements;

        public Action<int> OnTimerTick;
        public Action<int> OnTimeReduce;

        public bool IsDone => _levelTargets.Values.Sum() == 0;
        public bool IsValid => true;

        public GameplayModel(ILevelContent levelContent)
        {
            _levelElements = levelContent.GetLevelElements();
            _levelTargets = new Dictionary<string, int>();
            _levelTargetElements = new Dictionary<string, ILevelElement>();
            _targetUpdateHandlers = new Dictionary<string, Action<int>>();

            foreach (var item in _levelElements)
            {
                if (item.IsTarget)
                {
                    _levelTargets.Add(item.Id, item.Amount);
                    _levelTargetElements.Add(item.Id, item);
                }
            }
        }

        public ILevelElement[] GetLevelElements()
        {
            return _levelElements;
        }

        public void RemoveTarget(string id, int amount = 1)
        {
            _levelTargets[id] -= amount;
            _targetUpdateHandlers[id].Invoke(_levelTargets[id]);
        }

        public void AddTarget(string id, int amount)
        {
            _levelTargets[id] += amount;
            _targetUpdateHandlers[id].Invoke(_levelTargets[id]);
        }

        public void AssignTargetUpdateHandler(string id, Action<int> handler)
        {
            if (!_targetUpdateHandlers.TryAdd(id, handler))
            {
                _targetUpdateHandlers[id] += handler;
            }
        }

        public int GetTargetAmount(string id)
        {
            return _levelTargets[id];
        }

        public string[] GetTargets()
        {
            return _levelTargets.Keys.ToArray();
        }

        public ILevelElement GetTargetAsLevelElementById(string id)
        {
            return _levelTargetElements[id];
        }

        public void TickTimer(int seconds)
        {
            OnTimerTick?.Invoke(seconds);
        }

        public void ReduceTime(int timeReduce)
        {
            OnTimeReduce?.Invoke(timeReduce);
        }
    }
}