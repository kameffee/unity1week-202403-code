using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class HelpView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private Button _closeButton;

        public Observable<Unit> OnClickCloseAsObservable() => _closeButton.OnClickAsObservable();

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            var sequence = DOTween.Sequence();
            sequence.Join(_root.DOAnchorPosY(-48, 0.16f)
                .From()
                .SetEase(Ease.OutBack)
            );
            sequence.Join(_canvasGroup.DOFade(1, 0.1f).SetEase(Ease.Linear));
            sequence.SetUpdate(true);
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            await _canvasGroup.DOFade(0, 0.16f)
                .SetLink(gameObject)
                .SetUpdate(true)
                .WithCancellation(cancellationToken);
        }
    }
}