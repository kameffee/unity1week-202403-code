using System.Collections.Generic;
using Unity1week202403.Data;
using Unity1week202403.Domain;
using Unity1week202403.Domain.Capture;
using Unity1week202403.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Installer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private List<LifetimeScopeBuilder> _lifetimeScopeBuilders;

        [SerializeField]
        private TransitionView _transitionViewPrefab;

        [SerializeField]
        private StageMasterDataStoreSource _stageMasterDataStoreSource;

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (var lifetimeScopeBuilder in _lifetimeScopeBuilders)
            {
                lifetimeScopeBuilder.Configure(builder);
            }

            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.Register<CaptureTextureContainer>(Lifetime.Singleton);
            builder.RegisterBuildCallback(resolver => resolver.Resolve<CaptureTextureContainer>());

            builder.RegisterEntryPoint<UnscaledShaderTime>();

            BuildTransition(builder);
            BuildStageMasterData(builder);
        }

        private void BuildTransition(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab<TransitionView>(_transitionViewPrefab, Lifetime.Singleton)
                .DontDestroyOnLoad();
            builder.RegisterEntryPoint<TransitionPresenter>().AsSelf();

            builder.RegisterBuildCallback(resolver => resolver.Resolve<TransitionView>());
        }

        private void BuildStageMasterData(IContainerBuilder builder)
        {
            builder.Register<StageMasterDataRepository>(Lifetime.Singleton);
            builder.RegisterInstance<StageMasterDataStoreSource>(_stageMasterDataStoreSource);
        }
    }
}