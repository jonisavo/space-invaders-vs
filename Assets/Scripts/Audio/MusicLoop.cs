using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicLoop : MonoBehaviour
    {
        [Tooltip("Possible music tracks to play.")]
        public MusicTrack[] tracks;
        
        [Tooltip("Whether a random intro + loop should be played on awake.")]
        public bool playOnAwake;

        private AudioSource _audioSource;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            
            _audioSource.loop = true;

            if (playOnAwake)
                PlayRandom();
        }

        public void PlayRandom()
        {
            if (tracks.Length == 1)
                Play(tracks[0]);
            else
                Play(tracks[Random.Range(0, tracks.Length)]);
        }

        public void Play(MusicTrack track) => Play(track.intro, track.loop);

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