using System.Collections.Generic;
using System.Linq;
using Unity1week202403.Data;

namespace Unity1week202403.Domain
{
    public class SkillTargetCalculator
    {
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private readonly BattleMonsterContainer _battleMonsterContainer;

        public SkillTargetCalculator(
            BattleMonsterPresenterContainer battleMonsterPresenterContainer,
            BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
            _battleMonsterContainer = battleMonsterContainer;
        }

        public IEnumerable<BattleMonster> FindTargets(BattleMonster invoker, ActiveSkill activeSkill)
        {
            return TargetTypeToBattleMonsters(invoker, activeSkill.SkillTargetType);
        }

        public BattleMonster BestTargets(BattleMonster invoker, IEnumerable<BattleMonster> targets, ActiveSkill activeSkill)
        {
            switch (activeSkill.SkillType)
            {
                case SkillType.AttackDamage:
                    // 一番近いか
                    var presenter = _battleMonsterPresenterContainer.Get(invoker.BattleMonsterId);
                    return targets
                        .Select(monster => _battleMonsterPresenterContainer.Get(monster.BattleMonsterId))
                        .OrderBy(target => presenter.DistanceFrom(target))
                        .Select(monsterPresenter => _battleMonsterContainer.Get(monsterPresenter.BattleMonsterId))
                        .FirstOrDefault();
                case SkillType.Heal:
                    // 最大HPから見て一番HPが減っている妖怪を選出
                    return targets
                        .OrderByDescending(x => x.Hp.CurrentValue.DecreaseValue)
                        .FirstOrDefault();
                default:
                    return null;
            }
        }

        public IEnumerable<BattleMonster> FindTargetsByRange(BattleMonster invoker, ActiveSkill activeSkill)
        {
            var targets = TargetTypeToBattleMonsters(invoker, activeSkill.SkillTargetType);
            // 攻撃範囲内の対象を取得
            return FindByRange(invoker, targets, invoker.Parameter.AttackRange);
        }

        private IEnumerable<BattleMonster> TargetTypeToBattleMonsters(
            BattleMonster invoker,
            SkillTargetType skillTargetType)
        {
            var monsters = new List<BattleMonster>();
            if (skillTargetType.HasFlag(SkillTargetType.Self))
            {
                monsters.Add(invoker);
            }

            if (skillTargetType.HasFlag(SkillTargetType.Ally))
            {
                var allyWithoutSelf = _battleMonsterContainer
                    .GetAllyBattleMonstersBy(invoker.BattleMonsterId)
                    .Where(monster => !monster.Equals(invoker));

                monsters.AddRange(allyWithoutSelf);
            }

            if (skillTargetType.HasFlag(SkillTargetType.Enemy))
            {
                monsters.AddRange(_battleMonsterContainer.GetEnemyBattleMonstersBy(invoker.BattleMonsterId));
            }

            return monsters.Distinct().Where(monster => !monster.IsDead);
        }

        private IEnumerable<BattleMonster> FindByRange(
            BattleMonster invoker,
            IEnumerable<BattleMonster> monsters,
            float range)
        {
            var invokerPresenter = _battleMonsterPresenterContainer.Get(invoker.BattleMonsterId);

            var targets = new List<BattleMonster>();
            foreach (var monster in monsters)
            {
                if (monster.IsDead)
                    continue;

                var battleMonsterPresenter = _battleMonsterPresenterContainer.Get(monster.BattleMonsterId);

                // 発動者との距離を測る
                var distance = invokerPresenter.CalculateActualDistanceToCollider(battleMonsterPresenter);

                // 攻撃範囲外の場合は対象外
                if (distance > invokerPresenter.ActualAttackRange)
                    continue;

                targets.Add(monster);
            }

            return targets;
        }
    }
}