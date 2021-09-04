using System.Collections;
using RedBlueGames.NotNull;
using RedBlueGames.Tools.TextTyper;
using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    public class GameEndCanvas : BarCanvas
    {
        [NotNull]
        public RawImage topBarImage;

        [NotNull]
        public RawImage bottomBarImage;
        
        [Header("Game Manager")]
        [NotNull (IgnorePrefab = true)]
        public GameManager gameManager;

        [Header("Text Typers")]
        [NotNull]
        public TextTyper headerTextTyper;

        [NotNull]
        public TextTyper footerTextTyper;

        private void OnEnable() => GameManager.OnGameEnd += StartBringToFront;

        private void OnDisable() => GameManager.OnGameEnd -= StartBringToFront;

        private void StartBringToFront(SIVSPlayer winner, SIVSPlayer loser, VictoryReason reason) =>
            StartCoroutine(BringToFront(winner, loser, reason));

        private IEnumerator BringToFront(SIVSPlayer winner, SIVSPlayer loser, VictoryReason reason)
        {
            yield return new WaitForSeconds(4f);

            yield return CloseBars();

            var oneSecondWait = new WaitForSeconds(1f);
            
            topBarImage.CrossFadeColor(Color.black, 1.0f, false, false);
            bottomBarImage.CrossFadeColor(Color.black, 1.0f, false, false);

            yield return oneSecondWait;

            headerTextTyper.TypeText($"{winner.Name} won.");

            yield return oneSecondWait;
            
            footerTextTyper.TypeText("Press any key to continue.");

            while (!Input.anyKeyDown)
                yield return null;

            gameManager.LeaveGame();
        }

        private static string GetVictoryReasonText(VictoryReason reason, string loserNickName)
        {
            switch (reason)
            {
                case VictoryReason.Leave:
                    return $"{loserNickName} left.";
                case VictoryReason.Round5:
                    return "The last round was beaten.";
                case VictoryReason.LastStanding:
                    return $"{loserNickName} ran out of lives.";
                default:
                    return "Congratulations to the winner!";
            }
        }
    }
}
