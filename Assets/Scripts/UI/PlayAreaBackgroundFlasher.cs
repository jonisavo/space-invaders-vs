using System.Collections;
using UnityEngine;
using Photon.Realtime;

namespace SIVS
{
    [RequireComponent(typeof(PlayAreaBackgroundPlane))]
    public class PlayAreaBackgroundFlasher : MonoBehaviour
    {
        private int _actorNumber = -1;
        
        private PlayAreaBackgroundPlane _backgroundPlane;

        private PlayArea _playArea;

        private static readonly Color WhiteFlashColor = new Color(1f, 1f, 1, 0.65f);
        
        private static readonly Color YellowFlashColor = new Color(1f, 0.93f, 0.18f, 0.65f);

        private static readonly Color PurpleFlashColor = new Color(0.85f, 0f, 0.85f, 0.65f);

        private void Awake()
        {
            _backgroundPlane = GetComponent<PlayAreaBackgroundPlane>();
            _playArea = GetComponentInParent<PlayArea>();
        }

        private void OnEnable()
        {
            _playArea.OnInitialize += HandlePlayAreaInitialize;
            InvaderHealth.OnKill += HandleInvaderKill;
            Powerup.OnGet += HandlePowerupGet;
            UFOHealth.OnKill += HandleUFOKill;
            GameManager.OnRoundChange += HandleRoundChange;
        }

        private void OnDisable()
        {
            _playArea.OnInitialize -= HandlePlayAreaInitialize;
            InvaderHealth.OnKill -= HandleInvaderKill;
            Powerup.OnGet -= HandlePowerupGet;
            UFOHealth.OnKill -= HandleUFOKill;
            GameManager.OnRoundChange -= HandleRoundChange;
        }

        private void HandlePlayAreaInitialize(Player player, int playerNumber)
        {
            _actorNumber = player.ActorNumber;
        }

        private void HandleInvaderKill(int killerActorNumber)
        {
            if (killerActorNumber != _actorNumber)
                return;

            _backgroundPlane.Flash(WhiteFlashColor, 1, 0.05f);
        }

        private void HandlePowerupGet(int actorNumber)
        {
            if (actorNumber != _actorNumber)
                return;
            
            _backgroundPlane.Flash(YellowFlashColor, 3, 0.04f);
        }

        private void HandleUFOKill(int killerActorNumber)
        {
            if (killerActorNumber != _actorNumber)
                return;
            
            _backgroundPlane.Flash(PurpleFlashColor, 4, 0.04f);
        }

        private void HandleRoundChange(Player player, int round)
        {
            if (player.ActorNumber != _actorNumber || round <= 1)
                return;
            
            if (round == Match.FinalRound)
                StartCoroutine(RainbowFlash(5, 0.04f));
            else
                _backgroundPlane.Flash(WhiteFlashColor, 5, 0.03f);
        }

        private IEnumerator RainbowFlash(int count, float duration)
        {
            Color[] colors = {WhiteFlashColor, YellowFlashColor, PurpleFlashColor};

            var wait = new WaitForSeconds(duration * 2);
            
            for (var i = 0; i < count; i++)
            {
                _backgroundPlane.Flash(colors[(i + 1) % colors.Length], 1, duration);

                yield return wait;
            }
        }
    }
}
