using Photon.Pun;
using TMPro;
using UnityEngine;

namespace SIVS
{
    public class JoinRoomButton : MonoBehaviour
    {
        public TMP_InputField roomNameInputField;

        public Matchmaker matchmaker;

        public void JoinRoom()
        {
            var roomName = roomNameInputField.text.Trim();
            
            if (string.IsNullOrEmpty(roomName) || string.IsNullOrEmpty(PhotonNetwork.NickName.Trim()))
                return;

            matchmaker.JoinNamedRoom(roomName);
        }
    }
}