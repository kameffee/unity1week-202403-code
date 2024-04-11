namespace Unity1week202403.Domain
{
    public class PlaceRecordUseCase
    {
        private readonly BattleMonsterPlaceHistory _battleMonsterPlaceHistory;
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private readonly BattleMonsterFactory _battleMonsterFactory;
        private readonly BattleMonsterPresenterFactory _battleMonsterPresenterFactory;
        private readonly PlaceRecordFactory _placeRecordFactory;
        private readonly PlayerStatus _playerStatus;

        public PlaceRecordUseCase(
            BattleMonsterPlaceHistory battleMonsterPlaceHistory,
            BattleMonsterContainer battleMonsterContainer,
            BattleMonsterPresenterContainer battleMonsterPresenterContainer,
            BattleMonsterFactory battleMonsterFactory,
            BattleMonsterPresenterFactory battleMonsterPresenterFactory,
            PlaceRecordFactory placeRecordFactory,
            PlayerStatus playerStatus)
        {
            _battleMonsterPlaceHistory = battleMonsterPlaceHistory;
            _battleMonsterContainer = battleMonsterContainer;
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
            _battleMonsterFactory = battleMonsterFactory;
            _battleMonsterPresenterFactory = battleMonsterPresenterFactory;
            _placeRecordFactory = placeRecordFactory;
            _playerStatus = playerStatus;
        }

        public bool HasRecord()
        {
            return _battleMonsterPlaceHistory.HasRecord();
        }

        public void Record()
        {
            var allyBattleMonsters = _battleMonsterContainer.GetAllyBattleMonsters();
            var record = _placeRecordFactory.Create(allyBattleMonsters);
            _battleMonsterPlaceHistory.Set(record);
        }

        public void Restore()
        {
            if (!HasRecord())
            {
                return;
            }

            var record = _battleMonsterPlaceHistory.GetLast();
            foreach (var historyRecordData in record.GetRecords())
            {
                CreateBattleMonster(historyRecordData);
            }
        }

        private void CreateBattleMonster(HistoryRecordData historyRecordData)
        {
            var monster = _battleMonsterFactory.Create(
                historyRecordData.BattleMonsterId,
                historyRecordData.MonsterId,
                isAlly: true);

            var monsterPresenter = _battleMonsterPresenterFactory.Create(monster, historyRecordData.Position);

            _battleMonsterContainer.Add(monster);
            _battleMonsterPresenterContainer.Add(monsterPresenter);

            // コスト消費
            _playerStatus.CostStatus.Consume(monster.Cost);
        }

        public void Reset()
        {
            _battleMonsterPlaceHistory.Reset();
        }
    }
}