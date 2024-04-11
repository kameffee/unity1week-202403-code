using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class EndingCardView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private EndingCardElementView _elementPrefab;

        [SerializeField]
        private RectTransform _parent;

        private readonly List<EndingCardElementView> _elements = new();

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        public void ApplyViewModel(ViewModel viewModel)
        {
            foreach (var elementViewModel in viewModel.ElementViewModels)
            {
                var element = Instantiate(_elementPrefab, _parent);
                element.ApplyViewModel(elementViewModel);
                _elements.Add(element);
            }
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            await _canvasGroup.DOFade(1, 1)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await _canvasGroup.DOFade(0, 1)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public async UniTask PerformAsync(CancellationToken cancellation)
        {
            await _scrollRect.DOHorizontalNormalizedPos(1, _elements.Count * 4)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .WithCancellation(cancellation);
        }

        public class ViewModel
        {
            public EndingCardElementView.ViewModel[] ElementViewModels { get; }

            public ViewModel(EndingCardElementView.ViewModel[] elementViewModels)
            {
                ElementViewModels = elementViewModels;
            }
        }
    }
}