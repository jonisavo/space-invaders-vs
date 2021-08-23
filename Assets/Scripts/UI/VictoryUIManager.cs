using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    public class VictoryUIManager : MonoBehaviour
    {
        [Tooltip("The canvas to show when the game is over.")]
        public GameObject resultCanvas;

        [Tooltip("The victory header to write text on.")]
        public TMP_Text victoryHeaderText;

        [Tooltip("The text component to write the victory reason on.")]
        public TMP_Text victoryReasonText;

        [Tooltip("The button to select when the canvas is shown.")]
        public Button leaveButton;

        public void ShowVictoryScreen(string winnerNickName, string loserNickName, VictoryReason reason)
        {
            resultCanvas.SetActive(true);
            victoryHeaderText.text = $"{winnerNickName} won!";
            victoryReasonText.text = GetVictoryReasonText(reason, loserNickName);
            
            leaveButton.Select();
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
