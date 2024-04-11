using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class PlaceRecordFactory
    {
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        
        public PlaceRecordFactory(BattleMonsterPresenterContainer battleMonsterPresenterContainer)
        {
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
        }

        public BattleMonsterPlaceRecord Create(IEnumerable<BattleMonster> allyBattleMonsters)
        {
            var record = new BattleMonsterPlaceRecord();
            foreach (var battleMonster in allyBattleMonsters)
            {
                var recordData = new HistoryRecordData(
                    battleMonster.BattleMonsterId,
                    battleMonster.MonsterId,
                    GetPosition(battleMonster.BattleMonsterId));
                record.AddRecord(recordData);
            }

            return record;
        }

        private Vector3 GetPosition(BattleMonsterId battleMonsterId)
        {
            var monsterPresenter = _battleMonsterPresenterContainer.Get(battleMonsterId);
            return monsterPresenter.WorldPosition;
        }
    }
}