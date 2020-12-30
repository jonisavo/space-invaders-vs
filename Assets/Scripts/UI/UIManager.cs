using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace SIVS
{
    public class UIManager : MonoBehaviourPunCallbacks
    {
        public Texture lifeTexture;

        public GameObject resultCanvas;

        public TMP_Text victoryText;

        private Dictionary<string, int> _cachedLives;
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _cachedLives = new Dictionary<string, int>();
        }

        private void OnGUI()
        {
            DrawLives();
        }
        
        #endregion
        
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PlayerStats.Lives))
                _cachedLives[targetPlayer.NickName] = (int) changedProps[PlayerStats.Lives];
        }

        #endregion

        public void ShowVictoryScreen(string nickName)
        {
            resultCanvas.SetActive(true);
            victoryText.text = nickName + " won!";
        }

        private void DrawLives()
        {
            if (!PhotonNetwork.InRoom) return;
            
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
            {
                if (!_cachedLives.ContainsKey(entry.Value.NickName)) continue;
                
                var initialXCoord = entry.Key % 2 == 0 ? 948 : 52;

                for (var i = 0; i < _cachedLives[entry.Value.NickName]; i++)
                    GUI.DrawTexture(new Rect(entry.Key % 2 == 0 ? initialXCoord - 40 * i : initialXCoord + 40 * i, 
                            656, 32, 32), lifeTexture);
            }
        }
    }
}
