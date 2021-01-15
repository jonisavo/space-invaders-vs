using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicLoop : MonoBehaviour
    {
        [Tooltip("Possible intros to play. Each intro should have a matching loop.")]
        public AudioClip[] intros;

        [Tooltip("Possible loops to play. Each loop should have a matching intro.")]
        public AudioClip[] loops;

        [Tooltip("Whether a random intro + loop should be played on awake.")]
        public bool playOnAwake;

        private AudioSource _audioSource;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            
            _audioSource.loop = true;

            if (intros.Length != loops.Length)
            {
                Debug.LogError("Each intro must have a matching loop!");
                return;
            }

            if (playOnAwake)
                PlayRandom();
        }

        public void PlayRandom()
        {
            if (intros.Length == 1)
            {
                Play(intros[0], loops[0]);
            }
            else
            {
                var index = Random.Range(0, intros.Length);
                Play(intros[index], loops[index]);
            }
        }

        public void Play(AudioClip intro, AudioClip loop)
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();

            _audioSource.clip = loop;
            
            _audioSource.PlayOneShot(intro);
            
            _audioSource.PlayScheduled(AudioSettings.dspTime + intro.length);
        }
    }
}