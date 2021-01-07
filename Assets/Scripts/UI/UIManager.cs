using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace SIVS
{
    public class UIManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The texture to show for lives.")]
        public Texture lifeTexture;

        [Tooltip("The canvas to show when the game is over.")]
        public GameObject resultCanvas;

        [Tooltip("The text to write the result of a game on.")]
        public TMP_Text victoryText;

        [Tooltip("GUI style to write UI text with.")]
        public GUIStyle guiStyle;

        private Dictionary<string, int> _cachedLives;

        private GUIStyle _guiStyle;
        
        #region Unity Callbacks

        private void Awake()
        {
            _cachedLives = new Dictionary<string, int>();
        }

        private void OnGUI()
        {
            if (!PhotonNetwork.InRoom) return;
            
            DrawNames();
            DrawRounds();
            DrawLives();
            DrawPoints();
        }
        
        #endregion
        
        #region PUN Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PlayerStats.Lives))
                _cachedLives[targetPlayer.NickName] = (int) changedProps[PlayerStats.Lives];
        }

        #endregion

        public void ShowVictoryScreen(string nickName)
        {
            resultCanvas.SetActive(true);
            victoryText.text = $"{nickName} won!";
        }
        
        private void DrawNames()
        {
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
                GUI.Label(new Rect(entry.Key == 1 ? 40 : 540, 70, 500, 200),
                    entry.Value.NickName, guiStyle);
        }

        private void DrawRounds()
        {
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
            {
                if (!entry.Value.CustomProperties.ContainsKey(PlayerStats.CurrentRound))
                    continue;

                GUI.Label(new Rect(entry.Key == 1 ? 374 : 874, 70, 500, 200),
                    $"Round {PlayerStats.GetRound(entry.Value)}", guiStyle);
            }

        }

        private void DrawLives()
        {
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
            {
                if (!_cachedLives.ContainsKey(entry.Value.NickName))
                    continue;
                
                var initialXCoord = entry.Key % 2 == 0 ? 948 : 52;

                for (var i = 0; i < _cachedLives[entry.Value.NickName]; i++)
                    GUI.DrawTexture(new Rect(entry.Key % 2 == 0 ? initialXCoord - 40 * i : initialXCoord + 40 * i, 
                            656, 32, 32), lifeTexture);
            }
        }

        private void DrawPoints()
        {
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
                GUI.Label(new Rect(entry.Key == 1 ? 52 : 902, 708, 500, 200),
                    entry.Value.GetScore().ToString("D5"), guiStyle);
        }
    }
}
