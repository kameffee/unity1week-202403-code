using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class HpGaugeView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Image _mainGauge;

        [SerializeField]
        private Image _subGauge;

        private Tween _tween;

        public void Apply(float toNormalizedValue)
        {
            _mainGauge.fillAmount = toNormalizedValue;
            _tween?.Kill();
            _tween = _subGauge.DOFillAmount(toNormalizedValue, 0.5f)
                .SetDelay(0.5f)
                .SetEase(Ease.OutSine);
        }
        
        public void SetActive(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1 : 0;
        }
    }
}