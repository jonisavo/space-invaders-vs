using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SIVS
{
    [RequireComponent(typeof(UIManager))]
    [RequireComponent(typeof(InvaderManager))]
    public class GameManager : MonoBehaviourPunCallbacks
    {
        private bool _bothReady = false;

        private UIManager _uiManager;

        private InvaderManager _invaderManager;
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            PlayerStats.InitializeStats();
            _uiManager = GetComponent<UIManager>();
            _invaderManager = GetComponent<InvaderManager>();
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PlayerStats.Ready))
            {
                if (!_bothReady && IsEveryoneReady())
                {
                    _bothReady = true;

                    if (PhotonNetwork.IsMasterClient)
                    {
                        _invaderManager.SpawnInvadersForAll();
                        _invaderManager.StartCoroutines();
                    }
                }
            }

            if (changedProps.ContainsKey(PlayerStats.Lives))
            {
                if ((int) changedProps[PlayerStats.Lives] <= 0)
                    EndGame(GetOtherPlayer(targetPlayer));
            }

            if (!changedProps.ContainsKey(PlayerStats.CurrentRound)) return;
            
            if ((int) changedProps[PlayerStats.CurrentRound] >= 6)
                EndGame(targetPlayer);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }

        #endregion

        public void LeaveGame() => PhotonNetwork.LeaveRoom();

        private bool IsEveryoneReady()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player == null) return false;

                if (!player.CustomProperties.ContainsKey(PlayerStats.Ready))
                    return false;

                if (!(bool)player.CustomProperties[PlayerStats.Ready]) return false;
            }
            
            return true;
        }
        
        private Player GetOtherPlayer(Player firstPlayer)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                if (player.ActorNumber != firstPlayer.ActorNumber)
                    return player;

            return null;
        }

        private void EndGame(Player winner)
        {
            if (winner == null)
                _uiManager.ShowVictoryScreen("No one");
            else
                _uiManager.ShowVictoryScreen(winner.NickName);
            
            StopAllCoroutines();
            
            DestroyBullets();

            if (!PhotonNetwork.IsMasterClient) return;
            
            _invaderManager.StopAllCoroutines();
            
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                invader.GetComponent<InvaderShoot>().StopShooting();
        }

        private void DestroyBullets()
        {
            foreach (var playerBullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                Destroy(playerBullet);

            foreach (var enemyBullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
                Destroy(enemyBullet);
        }
    }
}
