using Cysharp.Threading.Tasks;
using Unity1week202403.Structure;
using UnityEngine;

namespace Unity1week202403.Domain.Capture
{
    public class CaptureUseCase
    {
        private readonly Camera _camera;
        private readonly RenderTexture _renderTexture;
        private readonly CaptureTextureContainer _textureContainer;

        public CaptureUseCase(
            Camera camera,
            RenderTexture renderTexture,
            CaptureTextureContainer textureContainer)
        {
            _camera = camera;
            _renderTexture = renderTexture;
            _textureContainer = textureContainer;
        }

        public async UniTask Capture(StageId stageId)
        {
            _camera.gameObject.SetActive(true);
            _camera.Render();
            await UniTask.DelayFrame(1);
            _camera.gameObject.SetActive(false);

            RenderTexture.active = _renderTexture;
            var texture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            texture.Apply();
            RenderTexture.active = null;

            _textureContainer.Set(stageId, texture);
        }
    }
}