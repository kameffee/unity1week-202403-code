using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Unity1week202403.Data;
using Unity1week202403.Domain;
using UnityEngine;
using VContainer;

namespace Unity1week202403.Presentation
{
    public class BattleMonsterPresenter : MonoBehaviour
    {
        [SerializeField] private BattleMonsterView _view;

        public BattleMonsterId BattleMonsterId => _battleMonster.BattleMonsterId;

        [Inject]
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;

        [Inject]
        private readonly MonsterMasterDataService _monsterMasterDataService;

        [Inject]
        private readonly BattleMonsterAttackUseCase _battleMonsterAttackUseCase;

        [Inject]
        private readonly SkillTargetCalculator _skillTargetCalculator;

        private bool IsEnemy => _battleMonster.IsEnemy;
        private BattleMonster _battleMonster;

        private RaycastHit[] _raycastHits = new RaycastHit[10];
        private UniTask _interruptTask;

        public float ActualAttackRange => _battleMonster.Parameter.AttackRange +
                                           _monsterMasterDataService.GetColliderRadius(_battleMonster.MonsterId);

        public Vector2 Pos2D => new(transform.position.x, transform.position.z);
        public Vector3 WorldPosition => transform.position;
        public Vector3 CenterPosition => _view.CenterPosition;

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_battleMonster != null)
            {
                UnityEditor.Handles.Label(transform.position, $"{_battleMonster.Hp}/{_battleMonster.Parameter.Hp}");
                
                Gizmos.color = IsEnemy ? Color.red : Color.blue;
                Gizmos.DrawWireSphere(transform.position, _monsterMasterDataService.GetColliderRadius(_battleMonster.MonsterId));

                var attackRangeColor = Gizmos.color;
                attackRangeColor.a = 0.2f;
                Gizmos.color = attackRangeColor;
                Gizmos.DrawSphere(transform.position, ActualAttackRange);
            }
#endif
        }

        public void Initialize(BattleMonster battleMonster)
        {
            _battleMonster = battleMonster;

            _view.SetZPosition(_battleMonster.ZPosition);

            battleMonster.Hp
                .Subscribe(hp => _view.SetHpRate(hp.NormalizeValue))
                .AddTo(this);
            battleMonster.OnDead
                .SubscribeAwait((_, cancellationToken) => _view.DeadAsync(cancellationToken))
                .AddTo(this);
            battleMonster.OnDamaged
                .Subscribe(_ => _view.Damaged())
                .AddTo(this);
            battleMonster.OnHeal
                .Subscribe(heal => _view.Healed(heal))
                .AddTo(this);
            battleMonster.OnKnockback
                .Subscribe(knockback => _view.Knockback(knockback))
                .AddTo(this);
            battleMonster.OnUpdateAbnormalityEffects
                .Subscribe(abnormalityEffect => _view.UpdateAbnormality(abnormalityEffect))
                .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ => _battleMonster.UpdateTime(Time.time))
                .AddTo(this);

            _view.SetIsEnemy(IsEnemy);
        }

        public void Perform()
        {
            if (_battleMonster.IsDead)
            {
                return;
            }

            if (_interruptTask.Status == UniTaskStatus.Pending)
            {
                return;
            }

            // スキル対象者を探す (射程は考慮しない)
            var skillTargetMonsters = _skillTargetCalculator.FindTargets(_battleMonster, _battleMonster.ActiveSkill)
                .Where(monster => monster.BattleMonsterId != BattleMonsterId) // 自分は含めない
                .Select(monster => _battleMonsterPresenterContainer.Get(monster.BattleMonsterId))
                .ToArray();

            if (!skillTargetMonsters.Any())
                return;

            // スキル対象者の中で最も一番効果が高い対象を取得
            var bestTarget = _skillTargetCalculator.BestTargets(
                _battleMonster,
                skillTargetMonsters.Select(presenter => presenter._battleMonster),
                _battleMonster.ActiveSkill);

            var bestTargetPresenter = _battleMonsterPresenterContainer.Get(bestTarget.BattleMonsterId);

            if (CalculateActualDistanceToCollider(bestTargetPresenter) <= ActualAttackRange)
            {
                if (_battleMonster.IsAttackable())
                {
                    _interruptTask = AttackAsync(
                        bestTarget.BattleMonsterId,
                        _battleMonster.ActiveSkill,
                        this.GetCancellationTokenOnDestroy());
                }
            }
            else
            {
                // 動ける状態であれば移動する
                if (_battleMonster.IsMovable())
                {
                    ExeMoveTo(bestTargetPresenter);
                }
            }
        }

        public float CalculateActualDistanceToCollider(BattleMonsterPresenter target)
        {
            return CalculateActualDistanceToCollider(this, target);
        }

        private float CalculateActualDistanceToCollider(BattleMonsterPresenter invoker, BattleMonsterPresenter target)
        {
            var dir2D = target.Pos2D - invoker.Pos2D;
            var ray = new Ray(invoker.CenterPosition, new Vector3(dir2D.x, 0, dir2D.y));
            var hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, dir2D.magnitude, Const.LayerMaskMonsterCollider);

            for (var i = 0; i < hitCount; i++)
            {
                if (_raycastHits[i].transform.GetComponentInParent<BattleMonsterPresenter>() == target)
                {
                    return _raycastHits[i].distance;
                }
            }

            return float.MaxValue;
        }

        private void ExeMoveTo(BattleMonsterPresenter target)
        {
            _view.ExeMove(target.Pos2D - Pos2D, _battleMonster.Parameter.MoveSpeed);
        }

        private async UniTask AttackAsync(BattleMonsterId mainTargetMonsterId, ActiveSkill activeSkill,
            CancellationToken cancellationToken)
        {
            await _view.PreAttackAsync(_battleMonster.Parameter.PreAttackTime, IsEnemy, cancellationToken);

            IEnumerable<BattleMonsterId> subTargets = null;
            if (activeSkill.SkillRangeTargetType == SkillRangeTargetType.All)
            {
                subTargets = _skillTargetCalculator.FindTargetsByRange(_battleMonster, _battleMonster.ActiveSkill)
                    .Where(monster => monster.BattleMonsterId != mainTargetMonsterId)
                    .Select(monster => monster.BattleMonsterId);
            }

            var skillTarget = new SkillTarget(mainTargetMonsterId, subTargets);

            _battleMonsterAttackUseCase.Calculate(
                _battleMonster.BattleMonsterId,
                skillTarget,
                _battleMonster.ActiveSkill);

            await _view.PostAttackAsync(_battleMonster.Parameter.PostAttackTime, IsEnemy, cancellationToken);
        }

        public float DistanceFrom(BattleMonsterPresenter target)
        {
            return (Pos2D - target.Pos2D).magnitude;
        }

        public Vector3 Direction(BattleMonsterPresenter targetPresenter)
        {
            var normalized2D = (targetPresenter.Pos2D - Pos2D);
            return new Vector3(normalized2D.x, 0, normalized2D.y);
        }
    }
}