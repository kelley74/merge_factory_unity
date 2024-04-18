using System.Collections;
using DG.Tweening;
using Game.Configs.LevelData;
using Game.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Gameplay
{
    public class TargetCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Image _icon;
        [SerializeField] private Transform _doneIcon;
        [SerializeField] private float _doneDuration = 0.33f;
        [SerializeField] private float _updateDuration = 0.33f;
        [SerializeField] private float _doneDelay = 0.25f;
        [SerializeField] private float _doneStartScale = 1.2f;
        [SerializeField] private float _doneUpdateScale = 1.2f;
        [SerializeField] private string _iconAddressPrefix = "Icon";

        [Inject] private IAssetLoader _assetLoader;
        [Inject] private DiContainer _container;

        public void Init(ILevelElement element, int amount)
        {
            UpdateCard(amount, false);
            StartCoroutine(_assetLoader.LoadAsset<Sprite>($"{_iconAddressPrefix}/{element.Id}", LoadAndSetIcon, _container));
            gameObject.SetActive(true);
            _doneIcon.gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        }

        private void LoadAndSetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void UpdateCard(int amount)
        {
            UpdateCard(amount, true);
            if (amount == 0)
            {
                StartCoroutine(PlayDone());
            }
        }

        private IEnumerator PlayDone()
        {
            yield return new WaitForSeconds(_doneDelay);
            _doneIcon.gameObject.SetActive(true);
            _doneIcon.localScale = Vector3.one * _doneStartScale;
            _doneIcon.DOScale(1f, _doneDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(_doneDuration);
            transform.DOScale(0f, _doneDuration).SetEase(Ease.InOutBounce);
            gameObject.SetActive(false);
        }

        private void UpdateCard(int amount, bool animate)
        {
            _amountText.text = $"{amount}";
            if (animate)
            {
                transform.localScale = Vector3.one * _doneUpdateScale;
                transform.DOScale(1f, _updateDuration);
            }
        }
    }
}