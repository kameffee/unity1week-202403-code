using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202403.Domain;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class MonsterDamagePerformView : MonoBehaviour
    {
        [SerializeField]
        private Transform _bodyRoot;

        [SerializeField]
        private SpriteRenderer[] _targets = Array.Empty<SpriteRenderer>();

        [SerializeField]
        private string _damageSeName = "InGame/MonsterDamege";

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private static readonly int OverrideMixId = Shader.PropertyToID("_OverrideMix");

        private MaterialPropertyBlock _materialPropertyBlock;

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            var lifetimeScope = LifetimeScope.Find<LifetimeScope>();
            lifetimeScope.Container.Inject(this);
        }

        public async void Play()
        {
            _audioPlayer.PlaySe(_damageSeName);

            async UniTask DoAsync(CancellationToken cancellationToken)
            {
                const float interval = 0.04f;
                SetValue(_materialPropertyBlock, _targets, 1);
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: cancellationToken);
                SetValue(_materialPropertyBlock, _targets, 0);

                // ちょっと長めの待ち時間
                await UniTask.Delay(TimeSpan.FromSeconds(interval * 2), cancellationToken: cancellationToken);

                SetValue(_materialPropertyBlock, _targets, 1);
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: cancellationToken);
                SetValue(_materialPropertyBlock, _targets, 0);
            }

            await DoAsync(destroyCancellationToken);
        }

        private static void SetValue(MaterialPropertyBlock propertyBlock, SpriteRenderer[] targets, float value)
        {
            foreach (var target in targets)
            {
                var color = target.color;
                color.r = 1 - value;
                target.color = color;
            }
        }

        private void OnValidate()
        {
            _targets = _bodyRoot.GetComponentsInChildren<SpriteRenderer>();
        }
    }
}