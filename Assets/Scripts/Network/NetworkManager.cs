using UnityEngine;

namespace SIVS
{
    [DisallowMultipleComponent]
    public class NetworkManager : MonoBehaviour
    {
        public virtual bool IsConnected { get; protected set; }
        
        private static NetworkManager _instance;

        public delegate void ConnectDelegate();

        public static event ConnectDelegate OnConnect;
        
        public delegate void DisconnectDelegate();

        public static event DisconnectDelegate OnDisconnect;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        public virtual bool Connect()
        {
            return false;
        }

        public static bool ConnectUsingCurrentManager()
        {
            if (!_instance)
                throw new UnassignedReferenceException("No NetworkManager is active!");

            return _instance.Connect();
        }

        public static bool IsConnectedUsingCurrentManager()
        {
            if (!_instance)
                throw new UnassignedReferenceException("No NetworkManager is active!");

            return _instance.IsConnected;
        }

        protected void EmitConnectedEvent() => OnConnect?.Invoke();
        
        protected void EmitDisconnectedEvent() => OnDisconnect?.Invoke();
    }
}
