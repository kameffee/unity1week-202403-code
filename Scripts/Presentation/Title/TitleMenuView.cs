using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class TitleMenuView : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _licenseButton;

        public Observable<Unit> OnClickStartButtonAsObservable() => _startButton.OnClickAsObservable();
        public Observable<Unit> OnClickLicenseButtonAsObservable() => _licenseButton.OnClickAsObservable();
    }
}