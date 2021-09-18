using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(MusicLoop))]
    public class RoundMusic : MonoBehaviour
    {
        public int round = 1;

        public MusicTrack track;

        private bool _reachedRound;

        private MusicLoop _musicLoop;

        private void Awake() => _musicLoop = GetComponent<MusicLoop>();

        private void OnEnable() => SIVSPlayer.OnRoundChange += HandleRoundChange;

        private void OnDisable() => SIVSPlayer.OnRoundChange -= HandleRoundChange;

        private void HandleRoundChange(SIVSPlayer player, int newRound)
        {
            if (!_reachedRound && newRound >= round)
                PlayMusic();
        }

        private void PlayMusic()
        {
            _musicLoop.Play(track);
            _reachedRound = true;
        }
    }
}