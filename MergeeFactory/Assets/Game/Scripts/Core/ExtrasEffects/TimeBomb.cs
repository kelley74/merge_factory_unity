using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Core.ExtrasEffects;
using UnityEngine;
using Zenject;

public class TimeBomb : MonoBehaviour, IExtraEffect
{
    [Inject] private GameMaster _gameMaster;
    [SerializeField] private float _timeReduce;
    
    public void Apply()
    {
        _gameMaster.ReduceRoundTime(_timeReduce);
    }
}
