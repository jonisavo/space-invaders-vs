using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Button))]
    public class CancelMatchmakingButton : MonoBehaviour, IMatchmakingCallbacks
    {
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();
        
        private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);

        private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

        public void OnJoinedRoom()
        {
            _button.interactable = true;
        }

        public void OnLeftRoom()
        {
            _button.interactable = false;
        }

        public void OnCreatedRoom() { }

        public void OnJoinRoomFailed(short _, string __) { }

        public void OnCreateRoomFailed(short _, string __) { }

        public void OnJoinRandomFailed(short _, string __) { }

        public void OnFriendListUpdate(List<FriendInfo> _) { }
    }
}