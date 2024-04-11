using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Unity1week202403.Domain;
using UnityEngine;
using UnityEngine.UI;
using Toggle = Unity1week202403.Presentation.UI.Toggle;

namespace Unity1week202403.Presentation
{
    public class InBattleTimeControlView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private GameObject _playButtonRoot;

        [SerializeField]
        private GameObject _stopButtonRoot;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _stopButton;

        [SerializeField]
        private Toggle _speedToggle;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public Observable<Unit> OnPlayButtonClickAsObservable() => _playButton.OnClickAsObservable();
        public Observable<Unit> OnStopButtonClickAsObservable() => _stopButton.OnClickAsObservable();

        public async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            await _canvasGroup.DOFade(1, 0.2f)
                .SetUpdate(true)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            return _canvasGroup.DOFade(0, 0.2f)
                .SetUpdate(true)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public void SetState(bool isPlaying)
        {
            _playButtonRoot.SetActive(!isPlaying);
            _stopButtonRoot.SetActive(isPlaying);
        }

        public Observable<int> OnSpeedChangedAsObservable() => _speedToggle.OnValueChangedAsObservable();

        public void SetTimeScaleIndex(TimeScaleType speed)
        {
            _speedToggle.SetIndex((int)speed);
        }
    }
}