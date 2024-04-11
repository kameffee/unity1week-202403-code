using System.Linq;
using Unity1week202403.Domain.Capture;
using Unity1week202403.Presentation;
using Unity1week202403.Structure;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Installer
{
    public class EndingLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private Texture2D _dummyTexture;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<EndingView>();
            builder.RegisterEntryPoint<EndingPresenter>();

            builder.Register<CreateEndingCardViewModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EndingCardView>();

#if UNITY_EDITOR
            builder.RegisterBuildCallback(resolver =>
            {
                var container = resolver.Resolve<CaptureTextureContainer>();
                if (container.Any()) return;

                // デバッグ用ダミーデータを生成
                foreach (var stageId in Enumerable.Range(1, 15).Select(x => new StageId(x)))
                {
                    var texture2D = new Texture2D(_dummyTexture.width, _dummyTexture.height, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
                    texture2D.SetPixels(_dummyTexture.GetPixels());
                    texture2D.Apply();
                    container.Set(stageId, texture2D);
                }
            });
#endif
        }
    }
}