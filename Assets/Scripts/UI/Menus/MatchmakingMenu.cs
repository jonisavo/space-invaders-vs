using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace SIVS
{
    public class MatchmakingMenu : Menu, IMatchmakingCallbacks
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            PhotonNetwork.AddCallbackTarget(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        
        public void OnJoinedRoom()
        {
            MakeInteractableAndSelectPrimaryElement();
        }

        public void OnLeftRoom()
        {
            if (CanvasGroup)
                CanvasGroup.interactable = false;
        }

        public void OnCreatedRoom() { }

        public void OnJoinRoomFailed(short _, string __) { }

        public void OnCreateRoomFailed(short _, string __) { }

        public void OnJoinRandomFailed(short _, string __) { }

        public void OnFriendListUpdate(List<FriendInfo> _) { }
    }
}