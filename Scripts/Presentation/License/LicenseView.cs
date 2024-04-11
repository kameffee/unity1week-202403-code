using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class LicenseView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _licenseText;

        [SerializeField]
        private Button _closeButton;

        public Observable<Unit> OnCloseAsObservable() => _closeButton.OnClickAsObservable();

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            var sequence = DOTween.Sequence();
            sequence.Join(_root.DOAnchorPosY(-48, 0.16f)
                .From()
                .SetEase(Ease.OutBack)
            );
            sequence.Join(_canvasGroup.DOFade(1, 0f));
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            await _canvasGroup.DOFade(0, 0f)
                .WithCancellation(cancellationToken);
        }

        public void SetLicenseText(string licenseData)
        {
            _licenseText.text = licenseData;
        }
    }
}