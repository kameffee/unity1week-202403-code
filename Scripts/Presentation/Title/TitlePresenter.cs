using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class TitlePresenter : Presenter, IInitializable
    {
        private readonly TitleMenuView _titleMenuView;
        private readonly AudioPlayer _audioPlayer;
        private readonly LicensePresenter _licensePresenter;
        private readonly SceneLoader _sceneLoader;

        public TitlePresenter(
            TitleMenuView titleMenuView,
            AudioPlayer audioPlayer,
            LicensePresenter licensePresenter,
            SceneLoader sceneLoader)
        {
            _titleMenuView = titleMenuView;
            _audioPlayer = audioPlayer;
            _licensePresenter = licensePresenter;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _audioPlayer.PlayBgm("Title");

            _titleMenuView.OnClickStartButtonAsObservable()
                .Subscribe(_ => LoadInGame().Forget())
                .AddTo(this);

            _titleMenuView.OnClickLicenseButtonAsObservable()
                .SubscribeAwait(async (_, token) => await _licensePresenter.ShowAsync(token))
                .AddTo(this);
        }

        private async UniTask LoadInGame()
        {
            await _sceneLoader.LoadAsync(Const.Scene.InGame);
        }
    }
}