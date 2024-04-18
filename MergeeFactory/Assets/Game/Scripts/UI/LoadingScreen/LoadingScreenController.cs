using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.UI.LoadingScreen
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeTime;
        [SerializeField] private GameObject _loadingObject;
        // ReSharper disable once IdentifierTypo
        private Tweener _tweener;

        public void Open()
        {
            _loadingObject.SetActive(true);
            _canvasGroup.blocksRaycasts = true; 
            _tweener?.Kill();
            _tweener = _canvasGroup.DOFade(0f, _fadeTime);
            _tweener.onComplete += () =>
            {
                _canvasGroup.blocksRaycasts = false; 
                _loadingObject.SetActive(false);
            };
        }

        public IEnumerator OpenWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Open();
            yield return new WaitForSeconds(_fadeTime);
        }

        public void Close(Action onComplete, bool instant = false)
        {
            _loadingObject.SetActive(true);
            _canvasGroup.blocksRaycasts = true;
            _tweener?.Kill();
            if (instant)
            {
                _canvasGroup.alpha = 1f;
                onComplete?.Invoke();
            }
            else
            {
                _tweener = _canvasGroup.DOFade(1f, _fadeTime);
                _tweener.onComplete += () =>
                {
                    onComplete?.Invoke();
                };
            }
        }
    }
}