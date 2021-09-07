using System.Collections;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PlayAreaBackgroundPlane))]
    public class PlayAreaBackgroundFlasher : MonoBehaviour
    {
        private int _playerNumber = -1;
        
        private PlayAreaBackgroundPlane _backgroundPlane;

        private PlayArea _playArea;

        private static readonly Color WhiteFlashColor = new Color(1f, 1f, 1, 0.65f);
        
        private static readonly Color YellowFlashColor = new Color(1f, 0.93f, 0.18f, 0.65f);

        private static readonly Color PurpleFlashColor = new Color(0.85f, 0f, 0.85f, 0.65f);

        private static readonly Color RedFlashColor = new Color(0.9f, 0.1f, 0.1f, 0.75f);

        private void Awake()
        {
            _backgroundPlane = GetComponent<PlayAreaBackgroundPlane>();
            _playArea = GetComponentInParent<PlayArea>();
        }

        private void OnEnable()
        {
            _playArea.OnInitialize += HandlePlayAreaInitialize;
            InvaderHealth.OnDeath += HandleInvaderKill;
            Powerup.OnGet += HandlePowerupGet;
            UFOHealth.OnKill += HandleUFOKill;
            SIVSPlayer.OnRoundChange += HandleRoundChange;
            PlayerHealth.OnHit += HandlePlayerHit;
        }

        private void OnDisable()
        {
            _playArea.OnInitialize -= HandlePlayAreaInitialize;
            InvaderHealth.OnDeath -= HandleInvaderKill;
            Powerup.OnGet -= HandlePowerupGet;
            UFOHealth.OnKill -= HandleUFOKill;
            SIVSPlayer.OnRoundChange -= HandleRoundChange;
            PlayerHealth.OnHit -= HandlePlayerHit;
        }

        private void HandlePlayAreaInitialize(SIVSPlayer player)
        {
            _playerNumber = player.Number;
        }

        private void HandleInvaderKill(int killerActorNumber, GameObject _)
        {
            if (killerActorNumber != _playerNumber)
                return;

            _backgroundPlane.Flash(WhiteFlashColor, 1, 0.05f);
        }

        private void HandlePlayerHit(SIVSPlayer player)
        {
            if (player.Number != _playerNumber)
                return;
            
            _backgroundPlane.Flash(RedFlashColor, 3, 0.07f);
        }

        private void HandlePowerupGet(int actorNumber)
        {
            if (actorNumber != _playerNumber)
                return;
            
            _backgroundPlane.Flash(YellowFlashColor, 3, 0.04f);
        }

        private void HandleUFOKill(int killerActorNumber)
        {
            if (killerActorNumber != _playerNumber)
                return;
            
            _backgroundPlane.Flash(PurpleFlashColor, 4, 0.04f);
        }

        private void HandleRoundChange(SIVSPlayer player, int round)
        {
            if (player.Number != _playerNumber || round <= 1)
                return;
            
            if (round == Match.FinalRound)
                StartCoroutine(RainbowFlash(5, 0.04f));
            else
                _backgroundPlane.Flash(WhiteFlashColor, 5, 0.03f);
        }

        private IEnumerator RainbowFlash(int count, float duration)
        {
            Color[] colors = { WhiteFlashColor, YellowFlashColor, PurpleFlashColor, RedFlashColor };

            var wait = new WaitForSeconds(duration * 2);
            
            for (var i = 0; i < count; i++)
            {
                _backgroundPlane.Flash(colors[(i + 1) % colors.Length], 1, duration);

                yield return wait;
            }
        }
    }
}
