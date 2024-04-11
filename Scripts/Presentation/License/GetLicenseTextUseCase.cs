using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class GetLicenseTextUseCase
    {
        private readonly TextAsset _licenseText;

        public GetLicenseTextUseCase(TextAsset textAsset)
        {
            _licenseText = textAsset;
        }

        public string Get() => _licenseText.text;
    }
}