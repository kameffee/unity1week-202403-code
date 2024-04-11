using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ResultBackgroundPerformView : UIBehaviour, IMaterialModifier
    {
        [SerializeField]
        private Graphic _graphic;

        [SerializeField]
        [Range(0, 5)]
        private float _progress = 1;

        [SerializeField]
        private Material _material;

        private Graphic graphic => _graphic ? _graphic : _graphic = GetComponent<Graphic>();

        private static readonly int ProgressId = Shader.PropertyToID("_Progress");

        private MaterialPropertyBlock _materialPropertyBlock;

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            await DOTween.To(
                    getter: () => _progress,
                    setter: value =>
                    {
                        _progress = value;
                        graphic.SetMaterialDirty();
                    },
                    endValue: 1f,
                    duration: 0.2f
                )
                .SetUpdate(true)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            await DOTween.To(
                    getter: () => _progress,
                    setter: value =>
                    {
                        _progress = value;
                        graphic.SetMaterialDirty();
                    },
                    endValue: 5f,
                    duration: 0.5f
                )
                .SetUpdate(true)
                .SetLink(gameObject)
                .WithCancellation(cancellationToken);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (graphic == null) return;
            _graphic.SetMaterialDirty();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_material != null) DestroyImmediate(_material);
            _material = null;

            if (graphic != null) _graphic.SetMaterialDirty();
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!IsActive() || graphic == null) return;
            graphic.SetMaterialDirty();
        }
#endif

        protected override void OnDidApplyAnimationProperties()
        {
            base.OnDidApplyAnimationProperties();
            if (!IsActive() || graphic == null) return;
            graphic.SetMaterialDirty();
        }

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            if (IsActive() == false || _graphic == null || !baseMaterial.HasProperty(ProgressId))
                return baseMaterial;

            // マテリアル複製
            if (_material == null)
            {
                _material = new Material(baseMaterial);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }

            // これまでのプロパティを引き継ぐ
            _material.CopyPropertiesFromMaterial(baseMaterial);

            _material.SetFloat(ProgressId, _progress);

            return _material;
        }
    }
}