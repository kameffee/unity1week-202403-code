using System.Collections.Generic;
using System.Linq;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(fileName = "StageMasterDataStoreSource", menuName = "Stage/DataStoreSource", order = 0)]
    public class StageMasterDataStoreSource : ScriptableObject
    {
        public IReadOnlyList<StageMasterData> Data => _data;

        [SerializeField]
        private List<StageMasterData> _data;

        public StageMasterData Get(StageId id) => _data.Find(data => data.Id == id);

        public StageMasterData[] GetAll() => _data.ToArray();

        public void Validate()
        {
            _data = _data.Distinct()
                .Where(data => data != null)
                .OrderBy(data => data.Id)
                .ToList();
        }

        public bool Exists(StageId id) => _data.Exists(data => data.Id == id);
    }
}