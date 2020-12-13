using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class SpawnManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            object[] playerData = { PhotonNetwork.NickName };
            PhotonNetwork.Instantiate("PlayerShip",
                new Vector3(0, -1.0f, 0), Quaternion.identity, 0, playerData);
        }
    }
}
