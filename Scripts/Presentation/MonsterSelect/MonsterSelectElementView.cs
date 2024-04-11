using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using R3.Triggers;
using TMPro;
using Unity1week202403.Structure;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class MonsterSelectElementView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private List<CanvasGroup> _tipsCanvasGroups;

        [Header("Normal Monster")]
        [SerializeField]
        private Image _thumbnail;

        [SerializeField]
        private TextMeshProUGUI _cost;

        [SerializeField]
        private Button _button;

        [Header("Big Monster")]
        [SerializeField]
        private Image _bigThumbnail;

        [SerializeField]
        private TextMeshProUGUI _bigCost;

        [SerializeField]
        private Button _bigButton;

        public MonsterId MonsterId => _viewModel.MonsterId;

        public bool HasBig => _viewModel.HasBig;
        public MonsterId BigMonsterId => _viewModel.BigViewModel.MonsterId;

        private ViewModel _viewModel;
        private bool _isVisibleBig;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            foreach (var group in _tipsCanvasGroups)
            {
                group.alpha = 0;
            }

            OnPointerEnterAsObservable()
                .Subscribe(_ => _tipsCanvasGroups.ForEach(group => group.alpha = 1))
                .AddTo(this);

            OnPointerExitAsObservable()
                .Subscribe(_ => _tipsCanvasGroups.ForEach(group => group.alpha = 0))
                .AddTo(this);
        }

        public Observable<MonsterId> OnClickAsObservable()
        {
            return _button.OnClickAsObservable()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Select(_ => _viewModel.MonsterId)
                .Merge(OnClickAsBigAsObservable());
        }

        public Observable<MonsterId> OnClickDetailAsObservable()
        {
            return _button.OnPointerClickAsObservable()
                .Where(pointerEventData => pointerEventData.button is PointerEventData.InputButton.Right) // 右クリック
                .Select(_ => _viewModel.MonsterId);
        }

        private Observable<MonsterId> OnClickAsBigAsObservable()
        {
            return _bigButton.OnClickAsObservable()
                .Select(_ => _viewModel.BigViewModel.MonsterId);
        }

        public Observable<Unit> OnPointerEnterAsObservable()
        {
            return _button.OnPointerEnterAsObservable()
                .Merge(_bigButton.OnPointerEnterAsObservable())
                .AsUnitObservable();
        }

        public Observable<Unit> OnPointerExitAsObservable()
        {
            return _button.OnPointerExitAsObservable()
                .Merge(_bigButton.OnPointerExitAsObservable())
                .AsUnitObservable();
        }

        public void ApplyViewModel(Cost usableCost, ViewModel elementViewModel)
        {
            _viewModel = elementViewModel;
            _thumbnail.sprite = elementViewModel.Thumbnail;
            _cost.text = elementViewModel.Cost.Value.ToString();
            _isVisibleBig = false;

            if (elementViewModel.HasBig)
            {
                _bigThumbnail.sprite = elementViewModel.BigViewModel.Thumbnail;
                _bigCost.text = elementViewModel.BigViewModel.Cost.Value.ToString();
            }

            SetSelectable(usableCost >= elementViewModel.Cost);
            SetBigSelectable(elementViewModel.HasBig && usableCost >= elementViewModel.BigViewModel.Cost);
            UpdateState();
        }

        public void SwitchBig(bool isBig)
        {
            _isVisibleBig = isBig;
            UpdateState();
        }

        private void UpdateState()
        {
            if (!_viewModel.HasBig)
            {
                _bigButton.gameObject.SetActive(false);
                return;
            }

            if (_isVisibleBig)
            {
                _button.gameObject.SetActive(false);
                _bigButton.gameObject.SetActive(true);
            }
            else
            {
                _button.gameObject.SetActive(true);
                _bigButton.gameObject.SetActive(false);
            }
        }

        public void SetSelect(bool isSelect)
        {
            var toY = isSelect ? 16 : 0;
            _root.DOAnchorPosY(toY, 0.2f)
                .SetLink(gameObject);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            await _canvasGroup.DOFade(1, 0.2f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            await _canvasGroup.DOFade(0, 0.2f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        private void SetSelectable(bool isSelectable) => _button.interactable = isSelectable;
        private void SetBigSelectable(bool isSelectable) => _bigButton.interactable = isSelectable;

        public class ViewModel
        {
            public MonsterId MonsterId { get; }
            public Sprite Thumbnail { get; }
            public Cost Cost { get; }
            public bool HasBig => BigViewModel != null;
            public ViewModel BigViewModel { get; }

            public ViewModel(MonsterId monsterId, Sprite thumbnail, Cost cost, ViewModel bigViewModel = null)
            {
                MonsterId = monsterId;
                Thumbnail = thumbnail;
                Cost = cost;
                BigViewModel = bigViewModel;
            }
        }

        public void UpdateUsableCost(Cost usableCost)
        {
            SetSelectable(usableCost >= _viewModel.Cost);
            if (_viewModel.HasBig)
            {
                _bigButton.interactable = usableCost >= _viewModel.BigViewModel.Cost;
            }
        }
    }
}