using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.UI.Gameplay
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _reduceTime;
        [SerializeField] private CanvasGroup _reduceTimeCanvasGroup;
        [SerializeField] private float _reduceTimeDelay = 0.5f;
        [SerializeField] private float _reduceTimeFadeDuration = 2f;

        private Tween _reduceTimeTween;
        private Tween _reduceTimeMoveTween;
        private Tween _reduceTimeColorTween;

        private void OnDisable()
        {
            _reduceTimeTween?.Kill();
            _reduceTimeCanvasGroup.alpha = 0f;
        }

        public void UpdateTime(int time)
        {
            _text.text = ConvertSecondsToTime(time);
        }

        public void ReduceTime(int seconds)
        {
            _reduceTime.text = $"-{seconds} sec";
            StartCoroutine(ReduceTimeWithDelay(_reduceTimeDelay));
        }

        private IEnumerator ReduceTimeWithDelay(float delay)
        {
            _text.color = Color.red;
            _reduceTimeColorTween?.Kill();
            _reduceTimeColorTween = _text.DOColor(Color.white, delay);
            _reduceTimeTween?.Kill();
            _reduceTimeCanvasGroup.transform.localPosition = Vector3.zero;
            _reduceTimeMoveTween?.Kill();
            _reduceTimeMoveTween = _reduceTimeCanvasGroup.transform.DOLocalMove(Vector3.up*300, _reduceTimeFadeDuration + delay);
            _reduceTimeCanvasGroup.alpha = 1f;
            yield return new WaitForSeconds(delay);
            _reduceTimeTween = _reduceTimeCanvasGroup.DOFade(0f, _reduceTimeFadeDuration).SetEase(Ease.OutBack);
        }

        private string ConvertSecondsToTime(int totalSeconds)
        {
            // Calculate hours, minutes, and seconds
            if (totalSeconds < 0)
            {
                totalSeconds = 0;
            }
            int hours = totalSeconds / 3600;
            int remainingSeconds = totalSeconds % 3600;
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;

            // Format the time string
            if (hours > 0)
            {
                return $"{hours:00}:{minutes:00}:{seconds:00}";
            }

            return $"{minutes:00}:{seconds:00}";
        }
    }
}