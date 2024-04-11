using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class BattleReadyPerformView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _rightPanel;

        [SerializeField]
        private RectTransform _leftPanel;

        [SerializeField]
        private CanvasGroup _stageNameCanvasGroup;

        [SerializeField]
        private TextMeshProUGUI _stageNameText;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            _stageNameCanvasGroup.alpha = 0;
        }

        public void QuickShow()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, 0.1f).SetEase(Ease.Linear));

            sequence.Append(
                _leftPanel.DOAnchorPosX(0, 0.2f)
                    .SetEase(Ease.OutCubic));
            sequence.Join(
                _rightPanel.DOAnchorPosX(0, 0.2f)
                    .SetEase(Ease.OutCubic));

            sequence.SetUpdate(true);
            sequence.WithCancellation(cancellationToken);

            await sequence.Play();
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            var sequence = DOTween.Sequence();
            // ちょい開け
            sequence.Append(
                _leftPanel.DOAnchorPosX(-((RectTransform)_leftPanel.parent).sizeDelta.x * 0.02f, 0.2f)
                    .SetEase(Ease.OutCubic));
            sequence.Join(
                _rightPanel.DOAnchorPosX(((RectTransform)_rightPanel.parent).sizeDelta.x * 0.02f, 0.2f)
                    .SetEase(Ease.OutCubic));

            sequence.AppendInterval(1);

            // 最後まで開ける
            sequence.Append(
                _leftPanel.DOAnchorPosX(-((RectTransform)_leftPanel.parent).sizeDelta.x, 2f)
                    .SetEase(Ease.InOutCubic));
            sequence.Join(
                _rightPanel.DOAnchorPosX(((RectTransform)_rightPanel.parent).sizeDelta.x, 2f)
                    .SetEase(Ease.InOutCubic));

            sequence.Append(_canvasGroup.DOFade(1, 0.1f).SetEase(Ease.Linear));

            sequence.SetUpdate(true);
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }

        public async UniTask PlayStageNamePerform(string stageName, CancellationToken cancellationToken = default)
        {
            _stageNameText.SetText(stageName);

            _stageNameCanvasGroup.alpha = 1;

            // 2秒かクリックで進む
            await UniTask.WhenAny(
                UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: cancellationToken),
                UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken)
            );

            await _stageNameCanvasGroup.DOFade(0, 1)
                .SetUpdate(true)
                .WithCancellation(cancellationToken);
        }
    }
}