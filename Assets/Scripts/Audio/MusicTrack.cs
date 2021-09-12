using System;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [Serializable]
    public struct MusicTrack
    {
        [NotNull]
        public AudioClip intro;
        [NotNull]
        public AudioClip loop;
    }
}
