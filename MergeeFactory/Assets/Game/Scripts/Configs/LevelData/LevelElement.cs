using System;
using UnityEngine;

namespace Game.Configs.LevelData
{
    [Serializable]
    public class LevelElement : ILevelElement
    {
        [SerializeField] private LevelObject _levelObject;
        [SerializeField] private int _amount;
        [SerializeField] private bool _isTarget;
        
        public ILoadable GetLoadable()
        {
            return _levelObject;
        }

        int ILevelElement.Amount => _amount;
        bool ILevelElement.IsTarget => _isTarget;
        string ILevelElement.Id => GetLoadable().GetAddress();

    }
    
    public class LevelGeneratedElement : ILevelElement
    {
        public int Amount { get; private set; }
        public bool IsTarget { get; private set; }
        
        string ILevelElement.Id => GetLoadable().GetAddress();
        
        private readonly ILoadable _levelObject;
        
        public LevelGeneratedElement(ILoadable levelObject, int amount, bool isTarget)
        {
            _levelObject = levelObject;
            Amount = amount;
            IsTarget = isTarget;
        }
        
        public ILoadable GetLoadable()
        {
            return _levelObject;
        }
    }
}