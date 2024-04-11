using System.Collections.Generic;
using System.Linq;

namespace Unity1week202403.Domain
{
    public class KnockbackService
    {
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private readonly BattleMonsterContainer _battleMonsterContainer;

        public KnockbackService(BattleMonsterPresenterContainer battleMonsterPresenterContainer,
            BattleMonsterContainer battleMonsterContainer)
        {
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
            _battleMonsterContainer = battleMonsterContainer;
        }

        public void Apply(
            BattleMonsterId invoker,
            IEnumerable<BattleMonsterId> targets,
            KnockbackEffect knockbackEffect)
        {
            var invokerMonster = _battleMonsterContainer.Get(invoker);
            Apply(
                invokerMonster,
                targets.Select(monsterId => _battleMonsterContainer.Get(monsterId)),
                knockbackEffect
            );
        }

        private void Apply(BattleMonster invoker, IEnumerable<BattleMonster> targets, KnockbackEffect knockbackEffect)
        {
            foreach (var target in targets)
            {
                Apply(invoker, target, knockbackEffect);
            }
        }

        private void Apply(BattleMonster invoker, BattleMonster target, KnockbackEffect knockbackEffect)
        {
            if (knockbackEffect.IsKnockback)
            {
                var invokerPresenter = _battleMonsterPresenterContainer.Get(invoker.BattleMonsterId);
                var targetPresenter = _battleMonsterPresenterContainer.Get(target.BattleMonsterId);
                var direction = invokerPresenter.Direction(targetPresenter).normalized;
                var knockback = new Knockback(direction, knockbackEffect.KnockbackPower);
                target.Knockback(knockback);
            }
        }
    }
}