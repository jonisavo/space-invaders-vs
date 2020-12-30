using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class SpawnManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            object[] playerData = {PhotonNetwork.NickName};
            var playerIndex = 1;
            
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
                if (entry.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    playerIndex = entry.Key;
            
            PhotonNetwork.Instantiate("PlayerShip",
                new Vector3(playerIndex == 1 ? -2.5f : 2.65f, -1.0f, 0), 
                Quaternion.identity, 0, playerData);
        }
    }
}
