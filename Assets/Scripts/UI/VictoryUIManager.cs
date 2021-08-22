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

        public void ShowVictoryScreen(string nickName, VictoryReason reason)
        {
            resultCanvas.SetActive(true);
            victoryHeaderText.text = $"{nickName} won!";
            victoryReasonText.text = GetVictoryReasonText(reason);
            
            leaveButton.Select();
        }

        private static string GetVictoryReasonText(VictoryReason reason)
        {
            switch (reason)
            {
                case VictoryReason.Leave:
                    return "The other player left.";
                case VictoryReason.Round5:
                    return "The last round was beaten.";
                case VictoryReason.LastStanding:
                    return "The other player ran out of lives.";
                default:
                    return "Congratulations to the winner!";
            }
        }
    }
}
