using DG.Tweening;
using Game.Core.ExtrasEffects;
using Game.Services;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Core
{
    public interface IMatchItem
    {
        string Id { get; }
        bool DeckExclude { get; }
        bool IsTarget { get; }
        void MatchAt(Vector3 position);
        void AlignAt(Vector3 position);
        void Release();
    }

    public class LevelObjectController : MonoBehaviour, IMatchItem
    {
        [Inject] private GameObjectPool _pool;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private bool _excludeDeck;
        [SerializeField] private bool _isTarget;

        // TODO: Move to config file
        [SerializeField] private float _startScale = 0.4f;
        [SerializeField] private float _deckScale = 0.2f;
        [SerializeField] private float _matchDuration = 0.25f;
        [SerializeField] private float _collectDuration = 0.25f;
        [SerializeField] private float _alignDuration = 0.15f;
        [SerializeField] private float _matchScale = 1.5f;
        [SerializeField] private float _matchMoveOffset = 0.15f;

        string IMatchItem.Id => _id;
        bool IMatchItem.DeckExclude => _excludeDeck;
        bool IMatchItem.IsTarget => _isTarget;

        private Tween _moveTween;
        private Tween _scaleTween;
        private Tween _rotateTween;

        private string _id;
        private bool _placed;

        public void Place(string id)
        {
            gameObject.SetActive(true);
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
            _id = id;
            transform.localScale = Vector3.one * _startScale;
            _placed = true;
        }

        void IMatchItem.Release()
        {
            if (!_placed)
            {
                return;
            }

            var go = gameObject;
            go.SetActive(false);
            _pool.ReturnObject(_id, go);
            _placed = false;
        }

        void IMatchItem.MatchAt(Vector3 position)
        {
            ClearSequence();

            _scaleTween = transform.DOScale(_deckScale * _matchScale, _matchDuration);
            _moveTween = transform.DOMove(position + Vector3.forward * _matchMoveOffset, _matchDuration);
            _rotateTween = transform.DORotate(Vector3.zero, _matchDuration);
            _moveTween.onComplete = (this as IMatchItem).Release;
        }

        void IMatchItem.AlignAt(Vector3 position)
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMove(position, _alignDuration);
        }

        public void Collect(Vector3 spotPosition)
        {
            ClearSequence();
            _collider.enabled = false;
            _rigidbody.isKinematic = true;

            _scaleTween = transform.DOScale(_deckScale, _collectDuration);
            _moveTween = transform.DOMove(spotPosition, _collectDuration);
            _rotateTween = transform.DORotate(Vector3.zero, _collectDuration);

            var extras = GetComponents<IExtraEffect>();
            if (extras != null)
            {
                foreach (var extra in extras)
                {
                    extra.Apply();
                }
            }
        }

        private void ClearSequence()
        {
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
            }

            if (_moveTween != null)
            {
                _moveTween.Kill();
            }

            if (_rotateTween != null)
            {
                _rotateTween.Kill();
            }
        }
    }
}