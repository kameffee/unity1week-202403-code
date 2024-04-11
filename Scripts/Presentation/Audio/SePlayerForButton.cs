using Unity1week202403.Domain;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class SePlayerForButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private string _onClickSeKey;

        [Inject]
        private readonly AudioPlayer _audioPlayer;

        private void Start()
        {
            var lifetimeScope = LifetimeScope.Find<LifetimeScope>();
            lifetimeScope.Container.Inject(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_button.interactable) return;

            if (!string.IsNullOrEmpty(_onClickSeKey))
            {
                _audioPlayer?.PlaySe(_onClickSeKey);
            }
        }

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }

            Assert.IsNotNull(_button, "Buttonがnullです。");
        }
    }
}