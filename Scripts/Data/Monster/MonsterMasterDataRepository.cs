using System.Linq;
using Unity1week202403.Structure;

namespace Unity1week202403.Data
{
    public class MonsterMasterDataRepository
    {
        private readonly MonsterMasterDataStoreSource _dataStoreSource;

        public MonsterMasterDataRepository(MonsterMasterDataStoreSource dataStoreSource)
        {
            _dataStoreSource = dataStoreSource;
        }

        public MonsterMasterData Get(MonsterId id) => _dataStoreSource.Data.FirstOrDefault(data => data.Id == id);

        public MonsterMasterData[] GetAll() => _dataStoreSource.Data;
    }
}