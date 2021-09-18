using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace SIVS
{
    public class PhotonNicknameTextInputButton : MainMenuTextInputButton, IConnectionCallbacks
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

        protected override string GetExistingText() => PhotonNetwork.NickName;

        public override void OnTextInputConfirm(string input)
        {
            base.OnTextInputConfirm(input);
            
            if (IsInputValid(input))
                PhotonNetwork.NickName = input;
        }

        public void OnConnected() { }

        public void OnConnectedToMaster()
        {
            var nickName = PhotonNetwork.NickName;
            
            writtenTextLabel.text = nickName;
            
            DetermineShownLabel(nickName);
        }

        public void OnDisconnected(DisconnectCause _) { }

        public void OnRegionListReceived(RegionHandler _) { }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> _) { }

        public void OnCustomAuthenticationFailed(string _) { }
    }
}