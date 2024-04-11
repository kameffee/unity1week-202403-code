using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Unity1week202403.Structure;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class MonsterSelectView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Button _submitButton;

        [SerializeField]
        private Button _resetButton;

        [SerializeField]
        private Button _bigSwitchButton;
        
        [SerializeField]
        private Button _helpButton;

        [SerializeField]
        private Transform _holder;

        [SerializeField]
        private MonsterSelectElementView _elementPrefab;

        private readonly List<MonsterSelectElementView> _elements = new();
        private readonly Subject<MonsterSelectElementView> _onAdd = new();
        private readonly Subject<MonsterId> _onSelect = new();
        private readonly Subject<MonsterId> _onDetail = new();

        private bool _isBig;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            _onAdd.SelectMany(view => view.OnClickAsObservable())
                .Subscribe(id => _onSelect.OnNext(id))
                .AddTo(this);
            
            _onAdd.SelectMany(view => view.OnClickDetailAsObservable())
                .Subscribe(id => _onDetail.OnNext(id))
                .AddTo(this);
        }

        public Observable<Unit> OnSubmitAsObservable() => _submitButton.OnClickAsObservable();
        public Observable<Unit> OnResetAsObservable() => _resetButton.OnClickAsObservable();
        public Observable<Unit> OnBigSwitchAsObservable() => _bigSwitchButton.OnClickAsObservable();
        public Observable<MonsterId> OnSelectAsObservable() => _onSelect;
        public Observable<MonsterId> OnDetailAsObservable() => _onDetail;
        public Observable<Unit> OnHelpAsObservable() => _helpButton.OnClickAsObservable();

        public void ApplyViewModel(Cost usableCost, ViewModel viewModel)
        {
            ClearElements();

            // ビッグ対応妖怪がいる場合のみ表示
            _bigSwitchButton.gameObject.SetActive(viewModel.HasBig);

            foreach (var elementViewModel in viewModel.ElementViewModels)
            {
                var element = Instantiate(_elementPrefab, _holder);
                element.ApplyViewModel(usableCost, elementViewModel);
                _elements.Add(element);
                _onAdd.OnNext(element);
            }
        }

        public void SetActiveSubmitButton(bool isActive) => _submitButton.interactable = isActive;

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            await _canvasGroup.DOFade(1, 0.2f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);

            async UniTask DoShowAsync(MonsterSelectElementView view, float delay, CancellationToken cancellationToken)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
                await view.ShowAsync(cancellationToken);
            }

            await UniTask.WhenAll(_elements.Select((view, index) =>
                DoShowAsync(view, index * 0.1f, cancellationToken)));
        }

        public async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            await _canvasGroup.DOFade(0, 0.2f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public class ViewModel
        {
            public MonsterSelectElementView.ViewModel[] ElementViewModels { get; }
            public bool HasBig { get; }

            public ViewModel(MonsterSelectElementView.ViewModel[] elementViewModels)
            {
                ElementViewModels = elementViewModels;
                HasBig = elementViewModels.Any(model => model.HasBig);
            }
        }

        public void SwitchBig()
        {
            _isBig = !_isBig;
            foreach (var element in _elements)
            {
                element.SwitchBig(_isBig);
            }
        }

        public void Select(MonsterId monsterId)
        {
            foreach (var element in _elements)
            {
                if (_isBig && element.HasBig)
                    element.SetSelect(element.BigMonsterId == monsterId);
                else
                    element.SetSelect(element.MonsterId == monsterId);
            }
        }

        public void Deselect()
        {
            foreach (var element in _elements)
            {
                element.SetSelect(false);
            }
        }

        public void UpdateUsableCost(Cost usableCost)
        {
            foreach (var monsterSelectElementView in _elements)
            {
                monsterSelectElementView.UpdateUsableCost(usableCost);
            }
        }

        private void ClearElements()
        {
            foreach (var element in _elements)
            {
                Destroy(element.gameObject);
            }

            _elements.Clear();
        }
    }
}