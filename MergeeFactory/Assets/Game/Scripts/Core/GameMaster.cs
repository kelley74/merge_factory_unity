using System.Collections;
using System.Collections.Generic;
using Game.Configs.LevelData;
using Game.Services;
using Game.UI.Gameplay;
using Game.UI.LoadingScreen;
using Game.UI.ResultScreen;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class GameMaster
    {
        [Inject] private CoroutineManager _coroutineManager;
        [Inject] private LoadingScreenController _loadingScreen;
        [Inject] private GameplayScreenController _gameplayScreen;
        [Inject] private ResultScreenController _resultScreen;
        [Inject] private GameObjectPool _pool;
        [Inject] private LevelsConfig _levelsConfig;
        [Inject] private GameProcessor _gameProcessor;
        [Inject] private DeckHolder _deckHolder;

        private List<IMatchItem> _levelElements;
        private float _roundTimer;
        private GameplayModel _gameRound;

        private readonly float _finishDelay = 0.75f;
        private readonly float _spawnHeightMin = 0.5f;
        private readonly float _spawnHeightMax = 2.5f;

        public void CreateNewRound()
        {
            _coroutineManager.StartCoroutine(GameRoundCoroutine());
        }
        
        public void ReduceRoundTime(float timeReduce)
        {
            _roundTimer -= timeReduce;
            _gameRound.ReduceTime((int)timeReduce);
            _gameRound.TickTimer((int)_roundTimer);
        }

        private IEnumerator GameRoundCoroutine()
        {
            yield return null; // Avoid possible overhead
            var level = _levelsConfig.GetLevelContent(1);
            _gameRound = new GameplayModel(level);
            var distance = level.BoundDistance;
            yield return PrepareLevelObjects(_gameRound, distance);
            foreach (var extra in level.Extras)
            {
                yield return AddExtras(extra.amount, extra.extrasId, distance);
            }

            yield return _gameplayScreen.Open(_gameRound);
            yield return new WaitForSeconds(1f);
            yield return _loadingScreen.OpenWithDelay(0f);
            _deckHolder.Init(_gameplayScreen.FetchDeckSpots());
            _gameProcessor.Activate(_gameRound);
            _roundTimer = level.Time;
            float tickTime = 0;
            _gameRound.TickTimer((int)_roundTimer);
            while (!_gameRound.IsDone && _gameRound.IsValid && !_deckHolder.IsFull && _roundTimer > Mathf.Epsilon)
            {
                _roundTimer -= Time.deltaTime;
                tickTime += Time.deltaTime;
                if (tickTime > 1f)
                {
                    tickTime -= 1f;
                    _gameRound.TickTimer((int)_roundTimer);
                }

                yield return null;
            }

            _gameProcessor.Deactivate();
            yield return new WaitForSeconds(_finishDelay);
            yield return _resultScreen.Open(new ResultModel(_gameRound.IsDone));
            _gameplayScreen.Close();
            foreach (var levelElement in _levelElements)
            {
                levelElement.Release();
            }
        }

        private IEnumerator AddExtras(int amount, string id, float distance)
        {
            int repeats = amount;
            for (int i = 0; i < amount; i++)
            {
                _pool.GetGameObject(id, (levelObject) =>
                {
                    var controller = levelObject.GetComponent<LevelObjectController>();
                    controller.Place(id);
                    levelObject.transform.position =
                        new Vector3(Random.Range(-distance, distance), Random.Range(_spawnHeightMin, _spawnHeightMax),
                            Random.Range(-distance, distance));
                    _levelElements.Add(controller);
                    repeats--;
                });
            }
            yield return null;
            while (repeats > 0)
            {
                yield return null;
            }
        }

        private IEnumerator PrepareLevelObjects(GameplayModel model, float distance)
        {
            _levelElements = new List<IMatchItem>();
            var levelElements = model.GetLevelElements();
            var levelLoader = new LevelLoader();

            foreach (var element in levelElements)
            {
                levelLoader.ObjectsToLoad += element.Amount;
                for (int i = 0; i < element.Amount; i++)
                {
                    _pool.GetGameObject(element.Id, (levelObject) =>
                    {
                        var controller = levelObject.GetComponent<LevelObjectController>();
                        controller.Place(element.GetLoadable().GetAddress());
                        levelObject.transform.position =
                            new Vector3(Random.Range(-distance, distance), Random.Range(_spawnHeightMin, _spawnHeightMax),
                                Random.Range(-distance, distance));
                        levelLoader.ObjectsToLoad--;
                        _levelElements.Add(controller);
                    });
                }
            }

            while (levelLoader.ObjectsToLoad > 0)
            {
                yield return null;
            }
        }

        private class LevelLoader
        {
            public int ObjectsToLoad { get; set; }
        }
    }
}