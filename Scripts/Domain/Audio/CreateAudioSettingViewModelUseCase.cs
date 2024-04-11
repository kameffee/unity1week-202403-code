using Unity1week202403.Presentation;

namespace Unity1week202403.Domain
{
    using ViewModel = AudioSettingView.ViewModel;

    public class CreateAudioSettingViewModelUseCase
    {
        private readonly AudioSettingsService _audioSettingsService;

        public CreateAudioSettingViewModelUseCase(AudioSettingsService audioSettingsService)
        {
            _audioSettingsService = audioSettingsService;
        }

        public ViewModel Create()
        {
            return new ViewModel(
                _audioSettingsService.BgmVolume.Value,
                _audioSettingsService.SeVolume.Value);
        }
    }
}