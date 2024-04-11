using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class BattleResultFailedPerformView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Button _retryButton;
        
        [SerializeField]
        private ResultBackgroundPerformView _resultBackgroundPerformView;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        public Observable<Unit> OnClickRetryAsObservable() => _retryButton.OnClickAsObservable();

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            await _canvasGroup.DOFade(1, 0.2f)
                .SetUpdate(true)
                .WithCancellation(cancellationToken);
            
            await _resultBackgroundPerformView.ShowAsync(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            
            await _resultBackgroundPerformView.HideAsync(cancellationToken);

            await _canvasGroup.DOFade(0, 0.2f)
                .SetUpdate(true)
                .WithCancellation(cancellationToken);
        }
    }
}