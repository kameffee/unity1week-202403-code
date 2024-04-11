using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202403.Presentation
{
    [RequireComponent(typeof(AudioSource))]
    public class BgmPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private float _volume;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        public void Play(AudioClip audioClip, bool isLoop)
        {
            SetVolume(_volume);
            _audioSource.clip = audioClip;
            _audioSource.loop = isLoop;
            _audioSource.Play();
        }

        public async UniTask StopAsync(float fadeoutTime = 0)
        {
            if (fadeoutTime > 0f)
            {
                await _audioSource
                    .DOFade(0, fadeoutTime)
                    .Play();
            }

            _audioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            _volume = volume;
            _audioSource.volume = volume;
        }
    }
}