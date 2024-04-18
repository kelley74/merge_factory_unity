using System.Linq;
using BigCity.Scripts.UI.Base;
using UnityEngine;

namespace Game.UI.Gameplay
{
    public class GameplayScreenView : UiView
    {
        [SerializeField] private RectTransform[] _spots;
        [SerializeField] private Vector3 _itemWorldOffset;
        [SerializeField] private TargetCard[] _targetCards;
        [SerializeField] private Timer _timer;

        private GameplayModel _gameplayModel;

        public Vector3[] SpotPositions { get; private set; }

        public override void Show()
        {
            base.Show();
            _gameplayModel = (GameplayModel)_model;
            SpotPositions = _spots.Select(t => t.transform.position + _itemWorldOffset).ToArray();

            foreach (var card in _targetCards)
            {
                card.gameObject.SetActive(false);
            }

            var targets = _gameplayModel.GetTargets();
            for (int i = 0; i < targets.Length; i++)
            {
                var card = _targetCards[i];
                var id = targets[i];
                card.gameObject.SetActive(true);
                card.Init(_gameplayModel.GetTargetAsLevelElementById(id), _gameplayModel.GetTargetAmount(id));
                _gameplayModel.AssignTargetUpdateHandler(id, card.UpdateCard);
            }

            _gameplayModel.OnTimerTick += _timer.UpdateTime;
            _gameplayModel.OnTimeReduce += _timer.ReduceTime;
        }

        public override void Hide()
        {
            base.Hide();
            _gameplayModel.OnTimerTick -= _timer.UpdateTime;
            _gameplayModel.OnTimeReduce -= _timer.ReduceTime;
        }
    }
}