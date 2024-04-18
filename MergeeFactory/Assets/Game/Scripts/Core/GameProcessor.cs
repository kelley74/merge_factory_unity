using System;
using Game.UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class GameProcessor : MonoBehaviour
    {
        [Inject] private DeckHolder _deckHolder;

        private bool _isActive;
        private ObjectClickHandler _clickHandler;
        private GameplayModel _gameplayModel;
        
        private void Start()
        {
            _clickHandler = new ObjectClickHandler(Camera.main);
        }

        public void Activate(GameplayModel gameplayModel)
        {
            _isActive = true;
            _gameplayModel = gameplayModel;
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        private void Update()
        {
            // Process the Game
            if (!_isActive)
            {
                return;
            }

            if (_clickHandler.CheckObjectClick<LevelObjectController>(out var levelObject))
            {
                var position = _deckHolder.GetNextPosition();
                levelObject.Collect(position);
                var matchItem = (IMatchItem)levelObject;
                
                if (matchItem.IsTarget)
                {
                    _gameplayModel.RemoveTarget(matchItem.Id);
                }
                
                if (!matchItem.DeckExclude)
                {
                    _deckHolder.RegisterNewItem(matchItem);
                }
                else
                {
                    matchItem.Release();
                }
            }
        }

        private class ObjectClickHandler
        {
            private readonly Camera _camera;

            public ObjectClickHandler(Camera camera)
            {
                _camera = camera;
            }

            public bool CheckObjectClick<T>(out T levelObject) where T : Component
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var hit))
                    {
                        GameObject hitObject = hit.collider.gameObject;
                        levelObject = hitObject.GetComponent<T>();
                        return levelObject != null;
                    }
                }

                levelObject = null;
                return false;
            }
        }
    }
}