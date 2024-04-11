using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(fileName = "StageSceneDataStoreSource", menuName = "Stage/StageSceneDataStoreSource")]
    public class StageSceneDataStoreSource : ScriptableObject
    {
        public IReadOnlyList<SceneObject> Scenes => _data;

        [SerializeField]
        private SceneObject[] _data;

        public string GetScene(int index) => _data[index];

        public void Validate()
        {
            _data = _data.Distinct()
                .Where(data => data != null)
                .ToArray();
        }
    }
}