using System.Collections.Generic;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(fileName = "StageMasterData_", menuName = "Stage/MasterData", order = 0)]
    public class StageMasterData : ScriptableObject, IComparer<StageMasterData>
    {
        public StageId Id => new(_id);
        public string StageName => _stageName;
        public SceneObject StageScene => _stageScene;
        public IReadOnlyList<StageMonsterData> StageMonsterData => _stageMonsterList;
        public IReadOnlyList<int> PlayerPlaceableMonsterList => _playerPlaceableMonsterList;
        public int PlayerCost => _playerCost;

        [SerializeField, Multiline(5)]
        private string _debugMemo;

        [SerializeField]
        private int _id;

        [SerializeField]
        private string _stageName;

        [SerializeField]
        private SceneObject _stageScene;

        [SerializeField]
        private List<StageMonsterData> _stageMonsterList;

        [SerializeField]
        private int _playerCost;

        [SerializeField]
        private List<int> _playerPlaceableMonsterList;

        public bool HasStageScene() => _stageScene is { IsEmpty: false };

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_stageName))
            {
                _stageName = $"Stage {_id}";
            }
        }

        public int Compare(StageMasterData x, StageMasterData y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.Id.CompareTo(y.Id);
        }
    }
}