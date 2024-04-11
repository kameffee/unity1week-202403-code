using System;
using UnityEngine;

namespace Unity1week202403.Data
{
    [Serializable]
    public class AudioClipData
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private AudioClip _clip;

        public string Id => _id;
        public AudioClip Clip => _clip;
    }
}