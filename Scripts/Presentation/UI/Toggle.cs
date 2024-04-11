using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation.UI
{
    public class Toggle : MonoBehaviour
    {
        [SerializeField]
        private List<Button> _toggleButtons;

        [SerializeField]
        private RectTransform _focusObject;

        private int _currentIndex;

        public Observable<int> OnValueChangedAsObservable()
        {
            return _toggleButtons
                .Select((button, index) => button.OnClickAsObservable().Select(_ => index))
                .Merge();
        }

        public void SetIndex(int index)
        {
            if (index < 0 || index >= _toggleButtons.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _currentIndex = index;
            var button = _toggleButtons[_currentIndex];
            _focusObject.anchoredPosition = ((RectTransform)button.transform).anchoredPosition;
        }
    }
}