using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class BattleStartPerformView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = true;

            await _canvasGroup.DOFade(1, 0f)
                .WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _canvasGroup.DOFade(0, 0.2f)
                .WithCancellation(cancellationToken);

            _canvasGroup.blocksRaycasts = false;
        }
    }
}