using Cysharp.Threading.Tasks;
using Unity1week202403.Presentation;

namespace Unity1week202403.Domain
{
    public class HelpAutoDisplayUseCase
    {
        private readonly HelpPresenter _helpPresenter;

        private bool _isRead;

        public HelpAutoDisplayUseCase(HelpPresenter helpPresenter)
        {
            _helpPresenter = helpPresenter;
        }

        private bool IsRead => _isRead;

        public async UniTask TryShowHelpAsync()
        {
            if (IsRead) return;

            // 既読
            SetRead();

            await _helpPresenter.ShowAsync();
        }

        private void SetRead()
        {
            _isRead = true;
        }
    }
}