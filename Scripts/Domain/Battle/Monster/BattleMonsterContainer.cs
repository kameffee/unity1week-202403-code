using System.Collections.Generic;
using System.Linq;
using R3;

namespace Unity1week202403.Domain
{
    public class BattleMonsterContainer
    {
        public Observable<BattleMonster> OnDead => _onDead;
        
        private readonly List<BattleMonster> _monsters = new();
        private readonly Subject<BattleMonster> _onDead = new();

        public void Add(BattleMonster monster)
        {
            _monsters.Add(monster);
            monster.OnDead.Subscribe(_ => _onDead.OnNext(monster));
        }

        public BattleMonster Get(BattleMonsterId battleMonsterId)
        {
            return _monsters.Find(x => x.BattleMonsterId == battleMonsterId);
        }

        public void Clear() => _monsters.Clear();

        public IEnumerable<BattleMonsterId> GetAllyIds()
        {
            return _monsters
                .Where(monster => monster.IsAlly)
                .Select(monster => monster.BattleMonsterId);
        }

        public IEnumerable<BattleMonsterId> GetEnemyIds()
        {
            return _monsters
                .Where(monster => monster.IsEnemy)
                .Select(monster => monster.BattleMonsterId);
        }

        public bool AnyAlly() => _monsters.Any(monster => monster.IsAlly);
        
        public IEnumerable<BattleMonster> GetAllyBattleMonsters()
        {
            return _monsters.Where(monster => monster.IsAlly);
        }

        /// <summary>
        /// 指定した妖怪が敵である妖怪を取得する
        /// </summary>
        public IEnumerable<BattleMonster> GetEnemyBattleMonstersBy(BattleMonsterId battleMonsterId)
        {
            var target = _monsters.Find(x => x.BattleMonsterId == battleMonsterId);
            return _monsters.Where(monster => target.IsEnemyTo(monster));
        }

        /// <summary>
        /// 指定した妖怪が味方である妖怪を取得する
        /// </summary>
        public IEnumerable<BattleMonster> GetAllyBattleMonstersBy(BattleMonsterId battleMonsterId)
        {
            var target = _monsters.Find(x => x.BattleMonsterId == battleMonsterId);
            return _monsters.Where(monster => target.IsAllyTo(monster));
        }

        public IEnumerable<BattleMonster> GetEnemyBattleMonsters()
        {
            return _monsters.Where(monster => monster.IsEnemy);
        }

        public void Remove(params BattleMonsterId[] battleMonsterIds)
        {
            foreach (var battleMonsterId in battleMonsterIds)
            {
                var monster = _monsters.First(monster => monster.BattleMonsterId == battleMonsterId);
                _monsters.Remove(monster);
            }
        }
    }
}