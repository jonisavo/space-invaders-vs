using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class NetworkManagerPhoton : NetworkManager, IConnectionCallbacks
    {
        public override bool IsConnected => PhotonNetwork.IsConnectedAndReady;

        private const string PhotonGameVersion = "1.2";

        private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);

        private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);
        
        public override bool Connect()
        {
            if (PhotonNetwork.IsConnected)
                return true;
            
            var value = PhotonNetwork.ConnectUsingSettings();
            
            PhotonNetwork.GameVersion = PhotonGameVersion;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0;
            
            return value;
        }

        public void OnConnected() { }

        public void OnConnectedToMaster()
        {
            EmitConnectedEvent();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            EmitDisconnectedEvent();
        }

        public void OnRegionListReceived(RegionHandler _) { }

        public void OnCustomAuthenticationFailed(string _) { }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> _) { }
    }
}
