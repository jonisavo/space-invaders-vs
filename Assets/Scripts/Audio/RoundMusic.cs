using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(MusicLoop))]
    public class RoundMusic : MonoBehaviourPunCallbacks
    {
        public int round = 1;

        public AudioClip intro;

        public AudioClip loop;

        private bool _reachedRound;

        private MusicLoop _musicLoop;

        private void Awake()
        {
            _musicLoop = GetComponent<MusicLoop>();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!changedProps.ContainsKey(PlayerStats.CurrentRound))
                return;
            
            if (!_reachedRound && (int) changedProps[PlayerStats.CurrentRound] >= round)
                PlayMusic();
        }

        private void PlayMusic()
        {
            _musicLoop.Play(intro, loop);
            _reachedRound = true;
        }
    }
}