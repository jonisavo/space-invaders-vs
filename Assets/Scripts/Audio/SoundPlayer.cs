using UnityEngine;

namespace SIVS
{
    public static class SoundPlayer
    {
        public static void PlaySound(AudioClip clip, float volumeScale = 1.0f)
        {
            GameObject.Find("Sound Player")
                .GetComponent<AudioSource>()
                .PlayOneShot(clip, volumeScale);
        }
    }
}