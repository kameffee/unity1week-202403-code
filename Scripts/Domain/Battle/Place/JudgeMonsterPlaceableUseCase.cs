using Unity1week202403.Presentation;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain
{
    public class JudgeMonsterPlaceableUseCase : IOnDrawGizmosHandler
    {
        private const float RaycastDistance = 10;

        private readonly MonsterMasterDataService _monsterMasterDataService;

        private GizmoData _gizmoData;

        public JudgeMonsterPlaceableUseCase(MonsterMasterDataService monsterMasterDataService)
        {
            _monsterMasterDataService = monsterMasterDataService;
        }

        public bool Judge(MonsterId monsterId, Vector3 worldPosition, bool isPlayer)
        {
            _gizmoData = default;

            if (IsPlaceableArea(worldPosition, isPlayer) == false)
            {
                return false;
            }

            return !IsOverlapByMonster(monsterId, worldPosition, out _gizmoData);
        }

        private static bool IsPlaceableArea(Vector3 worldPosition, bool isPlayer)
        {
            // プレイヤーは左半分側にしか置けない
            // 半径25の円がフィールド...22以下に配置可能
            if (isPlayer)
            {
                return worldPosition.x < 0 && new Vector2(worldPosition.x, worldPosition.z).magnitude < 22f;
            }

            return 0 < worldPosition.x;
        }

        private bool IsOverlapByMonster(MonsterId monsterId, Vector3 worldPosition, out GizmoData gizmoData)
        {
            var radius = _monsterMasterDataService.GetColliderRadius(monsterId);
            var origin = worldPosition + Vector3.up * RaycastDistance;
            var downRay = new Ray(origin, Vector3.down);

            var isHit = Physics.SphereCast(downRay, radius, out _, RaycastDistance, Const.LayerMaskMonsterCollider);
            gizmoData = new GizmoData
            {
                Radius = radius,
                Ray = downRay,
                WorldPosition = worldPosition,
                IsOverlap = isHit
            };

            return isHit;
        }

        private struct GizmoData
        {
            public Vector3 WorldPosition;
            public float Radius;
            public Ray Ray;
            public bool IsOverlap;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = _gizmoData.IsOverlap ? Color.red : Color.green;
            Gizmos.DrawWireSphere(
                _gizmoData.WorldPosition + Vector3.up * _gizmoData.Radius,
                _gizmoData.Radius);
            Gizmos.DrawRay(_gizmoData.Ray.origin, _gizmoData.Ray.direction * RaycastDistance);
        }
    }
}