using System;
using UnityEngine;

namespace Unity1week202403.Data
{
    [Serializable]
    public class SceneObject
    {
        public string SceneName => _sceneName;
        public bool IsEmpty => string.IsNullOrEmpty(_sceneName);

        [SerializeField]
        private string _sceneName;

        public SceneObject(string sceneName)
        {
            _sceneName = sceneName;
        }

        public static implicit operator string(SceneObject sceneObject)
        {
            return sceneObject._sceneName;
        }

        public static implicit operator SceneObject(string sceneName)
        {
            return new SceneObject(sceneName);
        }
    }
}