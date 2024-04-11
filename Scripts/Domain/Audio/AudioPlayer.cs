using System;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Presentation;
using UnityEngine;
using VContainer.Unity;
using AudioType = Unity1week202403.Data.AudioType;

namespace Unity1week202403.Domain
{
    public class AudioPlayer : IInitializable, IDisposable
    {
        private readonly BgmPlayer _bgm;
        private readonly SePlayer _se;
        private readonly AudioResourceLoader _loader;
        private readonly AudioSettingsService _audioSettingsService;
        private readonly CompositeDisposable _disposable = new();

        public AudioPlayer(
            AudioResourceLoader loader,
            AudioSettingsService audioSettingsService,
            BgmPlayer bgm,
            SePlayer sePlayer)
        {
            _bgm = bgm;
            _se = sePlayer;
            _loader = loader;
            _audioSettingsService = audioSettingsService;
        }

        void IInitializable.Initialize()
        {
            _audioSettingsService.BgmVolume
                .Subscribe(volume => _bgm.SetVolume(volume.Value))
                .AddTo(_disposable);

            _audioSettingsService.SeVolume
                .Subscribe(volume => _se.SetVolume(volume.Value))
                .AddTo(_disposable);
        }
        
        public void PlayBgm(string id, bool isLoop = true)
        {
            PlayBgmAsync(id, isLoop).Forget();
        }

        public async UniTask PlayBgmAsync(string id, bool isLoop = true)
        {
            var clip = await _loader.LoadAsync(AudioType.Bgm, id);
            _bgm.Play(clip, isLoop);
        }

        public async UniTask StopBgm(float duration = 1f)
        {
            await _bgm.StopAsync(duration);
        }

        public async void PlaySe(string id)
        {
            var clip = await _loader.LoadAsync(AudioType.Se, id);
            _se.PlayOneShot(clip);
        }
        
        public void PlaySe(AudioClip clip)
        {
            _se.PlayOneShot(clip);
        }

        public void Dispose() => _disposable.Dispose();
    }
}