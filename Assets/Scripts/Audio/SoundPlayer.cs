using UnityEngine;

namespace SIVS
{
    public static class SoundPlayer
    {
        private static AudioSource _audioSource;
        
        public static void PlaySound(AudioClip clip, float volumeScale = 1.0f)
        {
            if (!_audioSource)
                GetAudioSource();
            
            _audioSource?.PlayOneShot(clip, volumeScale);
        }

        private static void GetAudioSource() => _audioSource =
            GameObject.Find("Sound Player").GetComponent<AudioSource>();
    }
}