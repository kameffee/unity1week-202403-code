using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity1week202403.Presentation.UI
{
    public class ButtonAnimation : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField]
        private Button _target;

        private const float AnimationDuration = 0.18f;
        private const float OnEnterScaleMultiplier = 1.05f;
        private const float OnDownScaleMultiplier = 0.9f;

        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = _target.transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_target.interactable) return;
            _target.transform.DOScale(_defaultScale * OnEnterScaleMultiplier, AnimationDuration)
                .SetUpdate(true)
                .SetEase(Ease.OutQuad);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _target.transform.DOScale(_defaultScale, AnimationDuration)
                .SetUpdate(true)
                .SetEase(Ease.OutSine);
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_target.interactable) return;
            _target.transform.DOScale(_defaultScale * OnDownScaleMultiplier, AnimationDuration)
                .SetUpdate(true)
                .SetEase(Ease.OutCubic);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _target.transform.DOScale(_defaultScale, AnimationDuration)
                .SetUpdate(true);
        }

        private void OnValidate()
        {
            if (_target == null)
            {
                _target = GetComponent<Button>();
            }
        }
    }
}