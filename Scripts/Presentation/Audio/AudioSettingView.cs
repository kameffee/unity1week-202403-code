using R3;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class AudioSettingView : MonoBehaviour
    {
        [SerializeField]
        private BgmSettingView _bgmSettingView;

        [SerializeField]
        private SeSettingView _seSettingView;

        public void ApplyViewModel(ViewModel viewModel)
        {
            if (viewModel == null)
            {
                gameObject.SetActive(false);
                return;
            }

            _bgmSettingView.SetVolume(viewModel.BgmVolume);
            _seSettingView.SetVolume(viewModel.SeVolume);
        }

        public void SetBgmVolume(AudioVolume volume) => _bgmSettingView.SetVolume(volume);

        public void SetSfxVolume(AudioVolume volume) => _seSettingView.SetVolume(volume);

        public Observable<AudioVolume> OnChangeBgmVolumeAsObservable() => _bgmSettingView
            .OnChangeVolumeAsObservable()
            .Select(volume => new AudioVolume(volume));

        public Observable<AudioVolume> OnChangeSeVolumeAsObservable() => _seSettingView
            .OnChangeVolumeAsObservable()
            .Select(volume => new AudioVolume(volume));

        public Observable<Unit> OnPointerUpSeVolumeAsObservable() => _seSettingView.OnPointerUpAsObservable();

        public AudioClip GetSeSampleClip() => _seSettingView.SampleClip;

        public class ViewModel
        {
            public AudioVolume BgmVolume { get; }
            public AudioVolume SeVolume { get; }

            public ViewModel(AudioVolume bgmVolume, AudioVolume seVolume)
            {
                BgmVolume = bgmVolume;
                SeVolume = seVolume;
            }
        }
    }
}