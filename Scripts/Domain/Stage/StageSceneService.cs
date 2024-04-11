using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Unity1week202403.Domain
{
    public class StageSceneService : IInitializable
    {
        private int? _currentStageSceneId;

        public void Initialize()
        {
            _currentStageSceneId = null;

            if (TryFindLoadedSceneName(out var sceneName))
            {
                _currentStageSceneId = int.Parse(sceneName.Split('_')[1]);
            }
        }

        public bool IsLoaded()
        {
            return _currentStageSceneId != null && TryFindLoadedSceneName(out _);
        }

        public bool IsLoaded(string stageSceneName)
        {
            if (string.IsNullOrEmpty(stageSceneName))
            {
                throw new ArgumentNullException(nameof(stageSceneName));
            }
            
            return TryFindLoadedSceneName(out var loadedSceneName) && loadedSceneName == stageSceneName;
        }

        private bool TryFindLoadedSceneName(out string sceneName)
        {
            sceneName = Enumerable.Range(0, SceneManager.sceneCount)
                .Select(index => SceneManager.GetSceneAt(index).name)
                .FirstOrDefault(sceneName => sceneName.StartsWith("Stage_"));

            return sceneName != null;
        }

        public async UniTask LoadAsync(string stageSceneName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(stageSceneName))
            {
                throw new ArgumentNullException(nameof(stageSceneName));
            }

            // Stage_XXX の形式でない場合は読み込まない
            if (!int.TryParse(stageSceneName.Split('_')[1], out var stageSceneId))
            {
                Debug.LogError($"Invalid stage scene name: {stageSceneName}");
                return;
            }

            if (_currentStageSceneId == stageSceneId)
                return;

            await LoadAsync(stageSceneId, cancellationToken);
        }

        public async UniTask LoadAsync(int stageSceneId, CancellationToken cancellationToken = default)
        {
            if (stageSceneId < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(stageSceneId),
                    stageSceneId,
                    "0以上の値を指定してください。");
            }

            if (_currentStageSceneId == stageSceneId)
            {
                return;
            }

            var operation = SceneManager.LoadSceneAsync($"Stage_{stageSceneId:000}", LoadSceneMode.Additive);
            operation.WithCancellation(cancellationToken);
            await operation;

            _currentStageSceneId = stageSceneId;
        }

        public async UniTask UnloadAsync(CancellationToken cancellationToken)
        {
            if (_currentStageSceneId == null)
            {
                return;
            }

            var operation = SceneManager.UnloadSceneAsync(GetSceneName(_currentStageSceneId.Value));
            operation.WithCancellation(cancellationToken);
            await operation;

            _currentStageSceneId = null;
        }
        
        private static string GetSceneName(int stageSceneId)
        {
            return $"Stage_{stageSceneId:000}";
        }
    }
}