using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class TransitionView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Image _image;

        private Material _material;
        private static readonly int ProgressKey = Shader.PropertyToID("_Progress");

        public void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

#if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GetAssetPath(_image.material);
            if (!string.IsNullOrEmpty(path))
            {
                _image.material = new Material(_image.material);
            }
#endif
            _material = _image.material;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;
            await DOTween.To(
                    () => _material.GetFloat(ProgressKey),
                    value => _material.SetFloat(ProgressKey, value),
                    endValue: 0f,
                    duration: 1f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            _canvasGroup.blocksRaycasts = false;
            await DOTween.To(
                    () => _material.GetFloat(ProgressKey),
                    value => _material.SetFloat(ProgressKey, value),
                    endValue: 1f,
                    duration: 1f)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
            _canvasGroup.alpha = 0;
        }
    }
}