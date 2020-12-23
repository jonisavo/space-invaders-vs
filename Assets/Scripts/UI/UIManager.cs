using Photon.Pun;
using TMPro;
using UnityEngine;

namespace SIVS
{
    public class UIManager : MonoBehaviour
    {
        public Texture lifeTexture;

        public GameObject resultCanvas;

        public TMP_Text victoryText;

        private GameStatistics _statistics;
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            _statistics = GameObject.Find("Game Manager").GetComponent<GameStatistics>();
        }

        private void OnGUI()
        {
            DrawLives();
        }
        
        #endregion

        public void ShowVictoryScreen(string nickName)
        {
            resultCanvas.SetActive(true);
            victoryText.text = nickName + " won!";
        }

        private void DrawLives()
        {
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
            {
                var initialXCoord = entry.Key % 2 == 0 ? 948 : 52;
                for (var i = 0; i < _statistics.GetStatistics(entry.Value.NickName).Lives; i++)
                    GUI.DrawTexture(new Rect(entry.Key % 2 == 0 ? initialXCoord - 40 * i : initialXCoord + 40 * i, 
                            656, 32, 32), lifeTexture);
            }
        }
    }
}
