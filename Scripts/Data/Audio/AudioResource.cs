using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Unity1week202403.Data
{
    [CreateAssetMenu(fileName = "AudioResource", menuName = "Audio/AudioResource")]
    public class AudioResource : ScriptableObject
    {
        [SerializeField]
        private AudioClipData[] _bgmAudioDatas;

        [SerializeField]
        private AudioClipData[] _seAudioDatas;

        public AudioClipData Get(AudioType audioType, string id)
        {
            return audioType switch
            {
                AudioType.Bgm => GetBgm(id),
                AudioType.Se => GetSe(id),
                _ => throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null)
            };
        }

        private AudioClipData GetSe(string id)
        {
            var data = _seAudioDatas.FirstOrDefault(data => data.Id.Equals(id));
            if (data == null)
            {
                throw new FileNotFoundException($"AudioClip is not found. id: {id}");
            }

            return data;
        }

        private AudioClipData GetBgm(string id)
        {
            var data = _bgmAudioDatas.FirstOrDefault(data => data.Id.Equals(id));
            if (data == null)
            {
                throw new FileNotFoundException($"AudioClip is not found. id: {id}");
            }

            return data;
        }
    }
}