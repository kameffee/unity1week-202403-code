using System;
using Unity1week202403.Data;
using Unity1week202403.Domain;
using Unity1week202403.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Installer
{
    [Serializable]
    public class AudioLifetimeScope : LifetimeScopeBuilder
    {
        [SerializeField]
        private AudioResource _audioResource;

        public override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<AudioResource>(_audioResource);

            builder.Register<AudioResourceLoader>(Lifetime.Singleton);
            builder.Register<AudioPlayer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<AudioSettingsService>(Lifetime.Singleton);
            builder.Register<CreateAudioSettingViewModelUseCase>(Lifetime.Singleton);

            builder.RegisterComponentOnNewGameObject<BgmPlayer>(Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentOnNewGameObject<SePlayer>(Lifetime.Singleton).DontDestroyOnLoad();
        }
    }
}