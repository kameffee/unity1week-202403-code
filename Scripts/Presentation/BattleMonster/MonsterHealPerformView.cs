using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class MonsterHealPerformView : MonoBehaviour
    {
        [SerializeField]
        private Transform _bodyRoot;

        [SerializeField]
        private SpriteRenderer[] _targets = Array.Empty<SpriteRenderer>();

        private float _value;

        private void Start()
        {
            var lifetimeScope = LifetimeScope.Find<LifetimeScope>();
            lifetimeScope.Container.Inject(this);
        }

        public async void Play()
        {
            _value = 0;
            SetValue(_targets, 0);

            await DOTween.To(
                    getter: () => _value,
                    setter: value =>
                    {
                        _value = value;
                        SetValue(_targets, _value);
                    },
                    endValue: 1f,
                    duration: 1f)
                .WithCancellation(destroyCancellationToken);
        }

        private static void SetValue(SpriteRenderer[] targets, float value)
        {
            foreach (var target in targets)
            {
                var color = target.color;
                color.g = value;
                target.color = color;
            }
        }

        private void OnValidate()
        {
            _targets = _bodyRoot.GetComponentsInChildren<SpriteRenderer>();
        }
    }
}