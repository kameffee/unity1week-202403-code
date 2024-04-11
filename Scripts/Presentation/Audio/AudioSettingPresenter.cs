using R3;
using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class AudioSettingPresenter : Presenter, IInitializable
    {
        private readonly AudioSettingView _audioSettingView;
        private readonly CreateAudioSettingViewModelUseCase _createAudioSettingViewModelUseCase;
        private readonly AudioSettingsService _audioSettingsService;
        private readonly AudioPlayer _audioPlayer;

        public AudioSettingPresenter(
            AudioSettingView audioSettingView,
            CreateAudioSettingViewModelUseCase createAudioSettingViewModelUseCase,
            AudioSettingsService audioSettingsService,
            AudioPlayer audioPlayer)
        {
            _audioSettingView = audioSettingView;
            _createAudioSettingViewModelUseCase = createAudioSettingViewModelUseCase;
            _audioSettingsService = audioSettingsService;
            _audioPlayer = audioPlayer;
        }

        public void Initialize()
        {
            var viewModel = _createAudioSettingViewModelUseCase.Create();
            _audioSettingView.ApplyViewModel(viewModel);

            _audioSettingView.OnChangeBgmVolumeAsObservable()
                .Subscribe(volume => _audioSettingsService.SetBgmVolume(volume))
                .AddTo(this);

            _audioSettingView.OnChangeSeVolumeAsObservable()
                .Subscribe(volume => _audioSettingsService.SetSeVolume(volume))
                .AddTo(this);

            _audioSettingView.OnPointerUpSeVolumeAsObservable()
                .Subscribe(_ => _audioPlayer.PlaySe(_audioSettingView.GetSeSampleClip()))
                .AddTo(this);
        }
    }
}