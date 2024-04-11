using Unity1week202403.Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Installer
{
    public class TitleLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private Transform _uiRoot;

        [SerializeField]
        private LicenseView _licenseViewPrefab;

        [SerializeField]
        private TextAsset _licenseText;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<TitleMenuView>();
            builder.RegisterEntryPoint<TitlePresenter>();

            builder.RegisterEntryPoint<AudioSettingPresenter>();
            builder.RegisterComponentInHierarchy<AudioSettingView>();

            BuildLicense(builder);
        }

        private void BuildLicense(IContainerBuilder builder)
        {
            builder.Register<GetLicenseTextUseCase>(Lifetime.Scoped).WithParameter(_licenseText);
            builder.RegisterFactory<LicenseView>(_ => { return () => Instantiate(_licenseViewPrefab, _uiRoot); },
                Lifetime.Scoped);
            builder.RegisterEntryPoint<LicensePresenter>().AsSelf();
        }
    }
}