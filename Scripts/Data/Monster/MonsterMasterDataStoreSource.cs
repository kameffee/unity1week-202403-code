using System.Linq;
using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(fileName = "MonsterMasterDataStoreSource", menuName = "Monster/DataStoreSource", order = 0)]
    public class MonsterMasterDataStoreSource : ScriptableObject
    {
        public MonsterMasterData[] Data => _data;

        [SerializeField]
        private MonsterMasterData[] _data;

        public void Validate()
        {
            _data = _data.Distinct()
                .Where(data => data != null)
                .OrderBy(data => data.Id)
                .ToArray();
        }
    }
}