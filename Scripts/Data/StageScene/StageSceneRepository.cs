using System.Collections.Generic;
using System.Linq;

namespace Unity1week202403.Data
{
    public class StageSceneRepository
    {
        public IReadOnlyCollection<StageSceneData> Scenes { get; }

        public StageSceneRepository(StageSceneDataStoreSource source)
        {
            Scenes = source.Scenes.Select(scene => new StageSceneData(scene)).ToArray();
        }
    }
}