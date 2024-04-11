using System;
using R3;
using R3.Triggers;
using Unity1week202403.Data;
using Unity1week202403.Domain;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Presentation.UI
{
    [DisallowMultipleComponent]
    public class ButtonSePlayer : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private ButtonSePreset _buttonSePreset;

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private void Start()
        {
            var lifetimeScope = LifetimeScope.Find<LifetimeScope>();
            lifetimeScope.Container.Inject(this);

            _button.OnClickAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ => _audioPlayer.PlaySe(_buttonSePreset.ClickClip))
                .AddTo(this);

            _button.OnPointerEnterAsObservable()
                .Where(_ => _button.interactable)
                .Subscribe(_ => _audioPlayer.PlaySe(_buttonSePreset.HoverClip))
                .AddTo(this);
        }

        private void OnValidate()
        {
            if (_button == null) _button = GetComponent<Button>();
        }
    }
}