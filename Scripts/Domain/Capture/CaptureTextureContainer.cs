using System.Collections.Generic;
using Unity1week202403.Structure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity1week202403.Domain.Capture
{
    public class CaptureTextureContainer
    {
        private readonly Dictionary<StageId, Texture2D> _textures = new();

        public CaptureTextureContainer()
        {
            Debug.Log("Capture Container Created");
        }

        public bool Any() => _textures.Count > 0;

        public void Set(StageId stageId, Texture2D texture)
        {
            _textures[stageId] = texture;
        }

        public void Clear()
        {
            foreach (var texture in _textures.Values)
            {
                Object.Destroy(texture);
            }

            _textures.Clear();
        }

        public IEnumerable<BattleCaptureSet> All()
        {
            foreach (var pair in _textures)
            {
                yield return new BattleCaptureSet(pair.Key, pair.Value);
            }
        }
    }
}