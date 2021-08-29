using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class SpawnManagerOnline : SpawnManager
    {
        protected override void RespawnPlayer(SIVSPlayer player, GameObject playerObject)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != player.Number)
                return;
            
            base.RespawnPlayer(player, playerObject);
        }

        protected override void SpawnShips()
        {
            PhotonNetwork.Instantiate("PlayerShip",
                PlayerSpawnPoint(PhotonNetwork.LocalPlayer.ActorNumber), Quaternion.identity);
        }

        protected override void SpawnCover()
        {
            var playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            
            PhotonNetwork.Instantiate("Cover",
                PlayAreaPosition(playerNumber, -1.7f, -1.12f), Quaternion.identity);

            PhotonNetwork.Instantiate("Cover",
                PlayAreaPosition(playerNumber, 0.5f,-1.12f), Quaternion.identity);
        }
    }
}
