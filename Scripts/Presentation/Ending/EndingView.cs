using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class EndingView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Button _backToTitleButton;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        public Observable<Unit> OnClickBackToTitleAsObservable() => _backToTitleButton.OnClickAsObservable();

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1, 0.5f)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }
    }
}