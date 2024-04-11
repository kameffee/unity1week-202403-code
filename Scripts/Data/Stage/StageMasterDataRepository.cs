using Unity1week202403.Structure;

namespace Unity1week202403.Data
{
    public class StageMasterDataRepository
    {
        private readonly StageMasterDataStoreSource _source;

        public StageMasterDataRepository(StageMasterDataStoreSource source)
        {
            _source = source;
        }

        public StageMasterData Get(StageId id) => _source.Get(id);

        public StageMasterData[] GetAll() => _source.GetAll();
        
        public bool Exists(StageId id) => _source.Exists(id);
    }
}