namespace Unity1week202403.Domain
{
    public class BattleShutdownUseCase
    {
        private readonly TimeControlUseCase _timeControlUseCase;

        public BattleShutdownUseCase(TimeControlUseCase timeControlUseCase)
        {
            _timeControlUseCase = timeControlUseCase;
        }

        public void Shutdown()
        {
            _timeControlUseCase.SetDefaultTimeScale();
        }
    }
}