using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;

namespace Unity1week202403.Presentation
{
    public class EnemyGeneratePresenter : Presenter
    {
        private readonly GetStageInfoUseCase _getStageInfoUseCase;
        private readonly BattleMonsterPresenterFactory _battleMonsterPresenterFactory;
        private readonly BattleMonsterPresenterContainer _battleMonsterContainer;
        private readonly CreateEnemyBattleMonsterUseCase _createEnemyBattleMonsterUseCase;

        public EnemyGeneratePresenter(
            GetStageInfoUseCase getStageInfoUseCase,
            BattleMonsterPresenterFactory battleMonsterPresenterFactory,
            BattleMonsterPresenterContainer battleMonsterContainer,
            CreateEnemyBattleMonsterUseCase createEnemyBattleMonsterUseCase)
        {
            _getStageInfoUseCase = getStageInfoUseCase;
            _battleMonsterPresenterFactory = battleMonsterPresenterFactory;
            _battleMonsterContainer = battleMonsterContainer;
            _createEnemyBattleMonsterUseCase = createEnemyBattleMonsterUseCase;
        }

        public void Generate(StageId stageId)
        {
            var stageInfo = _getStageInfoUseCase.Get(stageId);

            foreach (var generateSet in stageInfo.MonsterGenerateSets)
            {
                var monster = _createEnemyBattleMonsterUseCase.CreateAndRegister(generateSet.MonsterId);
                var monsterPresenter = _battleMonsterPresenterFactory.Create(generateSet, monster);
                _battleMonsterContainer.Add(monsterPresenter);
            }
        }
    }
}