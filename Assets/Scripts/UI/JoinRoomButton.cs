using Photon.Pun;
using TMPro;
using UnityEngine;

namespace SIVS
{
    public class JoinRoomButton : MonoBehaviour
    {
        [Tooltip("The Input Field for the room name.")]
        public TMP_InputField roomNameInputField;

        [Tooltip("The Matchmaker script.")]
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