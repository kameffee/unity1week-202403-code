using Unity1week202403.Domain;

namespace Unity1week202403.Presentation
{
    public class BattleInitializer
    {
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private readonly TimeControlUseCase _timeControlUseCase;
        private readonly CameraController _cameraController;

        public BattleInitializer(
            BattleMonsterContainer battleMonsterContainer,
            BattleMonsterPresenterContainer battleMonsterPresenterContainer,
            TimeControlUseCase timeControlUseCase,
            CameraController cameraController)
        {
            _battleMonsterContainer = battleMonsterContainer;
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
            _timeControlUseCase = timeControlUseCase;
            _cameraController = cameraController;
        }

        public void Initialize()
        {
            _battleMonsterPresenterContainer.ClearAndDestroy();
            _battleMonsterContainer.Clear();

            // 元に戻しておく
            _timeControlUseCase.Play();

            _cameraController.SetPlayable(false);
            _cameraController.ResetPosition();
        }
    }
}