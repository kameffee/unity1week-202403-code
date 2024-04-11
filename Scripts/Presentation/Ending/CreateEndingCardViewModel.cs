using System;
using System.Linq;
using Unity1week202403.Data;
using Unity1week202403.Domain.Capture;

namespace Unity1week202403.Presentation
{
    using ViewModel = EndingCardView.ViewModel;
    using ElementViewModel = EndingCardElementView.ViewModel;

    public class CreateEndingCardViewModel
    {
        private readonly CaptureTextureContainer _captureTextureContainer;
        private readonly StageMasterDataRepository _stageMasterDataRepository;

        public CreateEndingCardViewModel(
            CaptureTextureContainer captureTextureContainer,
            StageMasterDataRepository stageMasterDataRepository)
        {
            _captureTextureContainer = captureTextureContainer;
            _stageMasterDataRepository = stageMasterDataRepository;
        }

        public ViewModel Create()
        {
            var elementViewModels = _captureTextureContainer.All()
                .Select(CreateElementViewModel)
                .ToArray();

            return new ViewModel(elementViewModels);
        }

        private ElementViewModel CreateElementViewModel(BattleCaptureSet set)
        {
            var stageMasterData = _stageMasterDataRepository.Get(set.StageId);
            return new ElementViewModel(set.Texture, stageMasterData.StageName);
        }
    }
}