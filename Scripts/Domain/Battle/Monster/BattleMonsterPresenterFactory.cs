using System;
using Unity1week202403.Data;
using Unity1week202403.Presentation;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class BattleMonsterPresenterFactory
    {
        private readonly MonsterMasterDataRepository _monsterMasterDataRepository;
        private readonly Func<GameObject, Vector3, BattleMonsterPresenter> _createFunc;

        public BattleMonsterPresenterFactory(
            MonsterMasterDataRepository monsterMasterDataRepository,
            Func<GameObject, Vector3, BattleMonsterPresenter> createFunc)
        {
            _monsterMasterDataRepository = monsterMasterDataRepository;
            _createFunc = createFunc;
        }

        public BattleMonsterPresenter Create(BattleMonster battleMonster, Vector3 worldPosition)
        {
            var prefab = _monsterMasterDataRepository.Get(battleMonster.MonsterId).Prefab;
            return Create(battleMonster.MonsterId, prefab, worldPosition, battleMonster);
        }

        public BattleMonsterPresenter Create(MonsterGenerateSet monsterGenerateSet, BattleMonster battleMonster)
        {
            return Create(
                monsterGenerateSet.MonsterId,
                monsterGenerateSet.Prefab,
                monsterGenerateSet.Position,
                battleMonster);
        }

        private BattleMonsterPresenter Create(
            MonsterId monsterId,
            GameObject prefab,
            Vector3 worldPosition,
            BattleMonster battleMonster)
        {
            var monster = _createFunc(prefab, worldPosition);
            monster.name = $"Monster_{battleMonster.BattleMonsterId.Value:0000}_{monsterId.Value:000}";
            monster.Initialize(battleMonster);
            return monster;
        }
    }
}