using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202403.Presentation
{
    public class EndingCardElementView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _stageName;
        
        [SerializeField]
        private RawImage _image;

        public void ApplyViewModel(ViewModel viewModel)
        {
            _image.texture = viewModel.Texture;
            _stageName.SetText(viewModel.StageName);
        }

        public class ViewModel
        {
            public Texture2D Texture { get; }
            public string StageName { get;}

            public ViewModel(Texture2D texture, string stageName)
            {
                Texture = texture;
                StageName = stageName;
            }
        }
    }
}