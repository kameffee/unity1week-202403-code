using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202403.Data;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;

namespace Unity1week202403.Domain
{
    public class LoadStageSceneUseCase
    {
        private readonly StageMasterDataRepository _stageMasterDataRepository;
        private readonly StageSceneRepository _stageSceneRepository;
        private readonly StageSceneService _stageSceneService;

        public LoadStageSceneUseCase(
            StageMasterDataRepository stageMasterDataRepository,
            StageSceneRepository stageSceneRepository,
            StageSceneService stageSceneService)
        {
            _stageMasterDataRepository = stageMasterDataRepository;
            _stageSceneRepository = stageSceneRepository;
            _stageSceneService = stageSceneService;
        }

        public async UniTask LoadAsync(StageId stageId, CancellationToken cancellationToken)
        {
            var stageMasterData = _stageMasterDataRepository.Get(stageId);

            // ステージシーンが設定されていない場合はランダムに選ぶ
            var stageSceneName = stageMasterData.HasStageScene()
                ? stageMasterData.StageScene.SceneName
                : _stageSceneRepository.Scenes.Shuffle().First().SceneName;

            // 次のシーンがすでに読み込まれている場合は何もしない
            if (_stageSceneService.IsLoaded(stageSceneName))
                return;

            // 何かしらのステージシーンが読み込まれている場合はアンロード
            if (_stageSceneService.IsLoaded())
            {
                await _stageSceneService.UnloadAsync(cancellationToken);
            }

            await _stageSceneService.LoadAsync(stageSceneName, cancellationToken);
        }
    }
}