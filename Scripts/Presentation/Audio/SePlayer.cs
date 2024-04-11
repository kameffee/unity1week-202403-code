using UnityEngine;

namespace Unity1week202403.Presentation
{
    [RequireComponent(typeof(AudioSource))]
    public class SePlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }
    }
}