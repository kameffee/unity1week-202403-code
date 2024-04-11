using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity1week202403.Domain
{
    public class SceneLoader
    {
        private readonly TransitionPresenter _transitionPresenter;
        private readonly ReactiveProperty<bool> _isLoading = new(false);

        public SceneLoader(TransitionPresenter transitionPresenter)
        {
            _transitionPresenter = transitionPresenter;
        }

        public async UniTask WaitAsync(CancellationToken cancellationToken = default)
        {
            // すでにロード中でない場合は即座に終了
            if (!_isLoading.Value)
                return;

            await _isLoading.Where(isLoading => !isLoading).FirstAsync(cancellationToken: cancellationToken);
        }


        public async UniTask LoadAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            _isLoading.Value = true;

            await _transitionPresenter.ShowAsync(cancellationToken);

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: cancellationToken);

            await Resources.UnloadUnusedAssets();

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: cancellationToken);

            await _transitionPresenter.HideAsync(cancellationToken);

            _isLoading.Value = false;
        }
    }
}