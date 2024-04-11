using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity1week202403.Domain;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    public class BattleMonsterView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _bodyRoot;
        [SerializeField] private Transform _damagedRoot;
        [SerializeField] private CapsuleCollider _capsuleCollider;
        [SerializeField] private AbnormalityView _abnormalityView;
        [SerializeField] private MonsterDamagePerformView _damagePerformView;
        [SerializeField] private MonsterHealPerformView _healPerformView;

        [SerializeField] private Transform _reverseTransform;
        [SerializeField] private BattleMonsterPrefabView _prefabView;

        [SerializeField] private HpGaugeView _hpGaugeView;

        public Vector3 CenterPosition => _capsuleCollider.transform.position + _capsuleCollider.center;

        public void SetHpRate(float current)
        {
            _hpGaugeView.Apply(current);
        }

        public void SetIsEnemy(bool isEnemy)
        {
            _reverseTransform.localScale = new Vector3(isEnemy ? 1 : -1, 1, 1);
            _prefabView?.SetUp(isEnemy);
        }

        public void ExeMove(Vector2 dir, float speed)
        {
            _prefabView?.PlayMove();
            dir = dir.normalized;
            var pos3D = new Vector3(dir.x, 0, dir.y);
            _rigidbody.MovePosition(_rigidbody.position + pos3D * 0.1f * speed * Time.deltaTime);
        }

        public async UniTask PreAttackAsync(float preAttackTime, bool isEnemy, CancellationToken cancellationToken)
        {
            _prefabView?.PlayAttack();
            await UniTask.Delay(TimeSpan.FromSeconds(preAttackTime), cancellationToken: cancellationToken);
            /*
            const float maxAngle = 15;
            var currentMaxAngle = isEnemy ? -maxAngle : maxAngle;
            await TaskByRate(preAttackTime, rate => _animRoot.localEulerAngles = new Vector3(0, 0, currentMaxAngle * MyMath.EaseOutCubic(rate)), cancellationToken);
            */
        }

        public async UniTask PostAttackAsync(float postAttackTime, bool isEnemy, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(postAttackTime), cancellationToken: cancellationToken);
            /*
            _prefabView.PlayMove();
            const float maxAngle = 15;
            var currentMaxAngle = isEnemy ? -maxAngle : maxAngle;
            await TaskByRate(postAttackTime, rate => _animRoot.localEulerAngles = new Vector3(0, 0, currentMaxAngle * (1 - MyMath.EaseOutBack(rate))), cancellationToken);
            */
        }

        public async UniTask DeadAsync(CancellationToken cancellationToken)
        {
            _prefabView?.PlayDeath();
            _rigidbody.isKinematic = true;
            const float deadSeconds = 0.5f;
            await UniTask.Delay(TimeSpan.FromSeconds(deadSeconds), cancellationToken: cancellationToken);

            await TaskByRate(deadSeconds, rate => _bodyRoot.localScale = Vector3.one * (1 - MyMath.EaseOutBack(rate)),
                cancellationToken);

            gameObject.SetActive(false);
        }

        public void Damaged()
        {
            _prefabView?.PlayDamaged();
            _damagedRoot.DOComplete();
            _damagedRoot.DOShakeRotation(0.5f, 20);
            _damagedRoot.DOShakeScale(0.5f, 0.1f);
            _damagePerformView.Play();
        }

        private static async UniTask TaskByRate(float time, Action<float> action, CancellationToken cancellationToken)
        {
            var startTime = Time.time;
            while (Time.time - startTime < time)
            {
                var rate = (Time.time - startTime) / time;
                action(rate);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            action(1);
        }

        public void Healed(Heal heal)
        {
            _healPerformView.Play();
        }

        public void Knockback(Knockback knockback)
        {
            _rigidbody.DOMove(
                    transform.position + knockback.Direction * knockback.Power,
                    0.2f)
                .SetEase(Ease.OutSine)
                .SetLink(gameObject);
        }

        public void UpdateAbnormality(AbnormalityTypeCollection abnormalityTypeCollection)
        {
            // Todo: エフェクト等を出す
            // Debug.Log($"[UpdateAbnormality] {string.Join<AbnormalityType>(", ", abnormalityTypeCollection.Types)}", gameObject);
            _abnormalityView.UpdateAbnormality(abnormalityTypeCollection);
        }

        public void SetZPosition(int zPosition)
        {
            var pos = transform.position;
            pos.z = zPosition / 1000f;
            //_bodyRoot.position = pos;
        }
    }
}