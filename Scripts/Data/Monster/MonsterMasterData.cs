using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(fileName = "MonsterMasterData_", menuName = "Monster/MonsterMasterData", order = 0)]
    public class MonsterMasterData : ScriptableObject
    {
        public MonsterId Id => new(_id);
        public string Name => _name;
        public string BattleFeature => _battleFeature;
        public string Description => _description;
        public Cost Cost => new(_cost);
        public MonsterParameter Parameter => _parameter;
        public SkillMasterData SkillMasterData => _skillMasterData;
        public GameObject Prefab => _prefab;
        public int ZPosition => _zPosition;
        public Sprite Thumbnail => _thumbnail;
        public MonsterMasterData BigMonsterMasterData => _bigMonsterMasterData;

        [SerializeField]
        private int _id;

        [SerializeField]
        private string _name;

        [SerializeField]
        [TextArea(1, 2)]
        private string _battleFeature;
        
        [SerializeField]
        [TextArea(3, 4)]
        private string _description;

        [SerializeField]
        private int _cost = 100;

        [SerializeField]
        private MonsterParameter _parameter;

        [SerializeField]
        private SkillMasterData _skillMasterData;

        [SerializeField]
        private GameObject _prefab;

        [Range(0, 1000)]
        [SerializeField]
        private int _zPosition;

        [SerializeField]
        private Sprite _thumbnail;

        [SerializeField]
        private MonsterMasterData _bigMonsterMasterData;
    }
}