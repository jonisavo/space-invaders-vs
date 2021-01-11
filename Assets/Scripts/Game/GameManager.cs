using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SIVS
{
    [RequireComponent(typeof(UIManager))]
    [RequireComponent(typeof(InvaderManager))]
    [RequireComponent(typeof(OptionsManager))]
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Audio clip to play upon a victory.")]
        public AudioClip victorySound;

        private bool _bothReady = false;

        private bool _gameOver = false;

        private InvaderManager _invaderManager;

        #region Unity Callbacks

        private void Awake()
        {
            PlayerStats.InitializeStats(PhotonNetwork.LocalPlayer);
            _invaderManager = GetComponent<InvaderManager>();
        }

        private void Start()
        {
            PlayerStats.SetReady(PhotonNetwork.LocalPlayer, true);
        }

        #endregion

        #region PUN Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (_gameOver) return;

            if (changedProps.ContainsKey(PlayerStats.Ready))
            {
                if (!_bothReady && IsEveryoneReady())
                {
                    _bothReady = true;

                    _invaderManager.InitializeInvaders();

                    if (PhotonNetwork.IsMasterClient)
                        Match.IsActive = true;
                }
            }

            if (changedProps.ContainsKey(PlayerStats.Lives))
            {
                if ((int) changedProps[PlayerStats.Lives] <= 0)
                    EndGame(GetOtherPlayer(targetPlayer), VictoryReasons.LastStanding);
            }

            if (!changedProps.ContainsKey(PlayerStats.CurrentRound))
                return;

            if ((int) changedProps[PlayerStats.CurrentRound] >= 6)
                EndGame(targetPlayer, VictoryReasons.Round5);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_gameOver) return;

            EndGame(GetOtherPlayer(otherPlayer), VictoryReasons.Leave);
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

                if (!(bool)player.CustomProperties[PlayerStats.Ready])
                    return false;
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

        private void EndGame(Player winner, VictoryReasons reason)
        {
            _gameOver = true;

            if (PhotonNetwork.IsMasterClient)
                Match.IsActive = false;

            PlayerStats.SetReady(PhotonNetwork.LocalPlayer, false);

            GetComponent<OptionsManager>().CloseCanvas();

            GetComponent<UIManager>()
                .ShowVictoryScreen(winner == null ? "No one" : winner.NickName, reason);

            GameObject.Find("Music Player")
                .GetComponent<AudioSource>()
                .Stop();

            if (winner != null && winner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                SoundPlayer.PlaySound(victorySound);

            SetHighScore();

            StopGameProcessing();
        }

        private void StopGameProcessing()
        {
            StopAllCoroutines();
            DestroyBullets();
            DestroyPowerups();
            _invaderManager.StopAllCoroutines();

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                invader.GetComponent<InvaderShoot>().StopShooting();
        }

        private void SetHighScore()
        {
            var highScore = PlayerPrefs.GetInt("HighScore", 0);

            var score = PhotonNetwork.LocalPlayer.GetScore();

            if (score > highScore)
                PlayerPrefs.SetInt("HighScore", score);
        }

        private void DestroyBullets()
        {
            foreach (var playerBullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                Destroy(playerBullet);

            foreach (var enemyBullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
                Destroy(enemyBullet);
        }

        private void DestroyPowerups()
        {
            foreach (var powerup in GameObject.FindGameObjectsWithTag("Powerup"))
            {
                var photonView = powerup.GetPhotonView();

                if (!photonView.IsMine) continue;

                photonView.RPC("DestroyPowerup", RpcTarget.All);
            }
        }
    }
}
