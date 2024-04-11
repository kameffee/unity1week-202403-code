using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class MonsterDetailView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private Image _thumbnail;

        [SerializeField]
        private Image _thumbnailBig;

        [SerializeField]
        private TextMeshProUGUI _monsterName;

        [SerializeField]
        private TextMeshProUGUI _battleFeature;

        [SerializeField]
        private TextMeshProUGUI _description;

        [SerializeField]
        private Button _closeButton;

        public Observable<Unit> OnClickCloseAsObservable() => _closeButton.OnClickAsObservable();

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        public void ApplyViewModel(ViewModel viewModel)
        {
            _thumbnail.sprite = viewModel.Thumbnail;
            _thumbnailBig.sprite = viewModel.Thumbnail;
            _battleFeature.text = viewModel.BattleFeature;
            _monsterName.text = viewModel.MonsterName;
            _description.text = viewModel.Description;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            await _canvasGroup.DOFade(1, 0.16f)
                .SetLink(gameObject)
                .SetUpdate(true)
                .WithCancellation(cancellationToken);
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

        public class ViewModel
        {
            public Sprite Thumbnail { get; }
            public string MonsterName { get; }
            public string BattleFeature { get; }
            public string Description { get; }

            public ViewModel(
                Sprite thumbnail,
                string monsterName,
                string battleFeature,
                string description)
            {
                Thumbnail = thumbnail;
                MonsterName = monsterName;
                BattleFeature = battleFeature;
                Description = description;
            }
        }
    }
}