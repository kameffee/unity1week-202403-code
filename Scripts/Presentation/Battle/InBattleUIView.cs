using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class InBattleUIView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Button _retryButton;

        [SerializeField]
        private Button _helpButton;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        public Observable<Unit> OnClickRetryAsObservable() => _retryButton.OnClickAsObservable();
        public Observable<Unit> OnClickHelpAsObservable() => _helpButton.OnClickAsObservable();

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            await _canvasGroup.DOFade(1, 0.2f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            await _canvasGroup.DOFade(0, 0.2f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }
    }
}