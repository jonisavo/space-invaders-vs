using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class PlayArea : MonoBehaviour
    {
        public Player Player { get; private set; }
        
        public int PlayerNumber { get; private set; }

        public delegate void OnInitializeDelegate(Player player, int playerNumber);

        public event OnInitializeDelegate OnInitialize;

        public void Initialize(Player player, int playerNumber)
        {
            Player = player;
            PlayerNumber = playerNumber;
            
            OnInitialize?.Invoke(player, playerNumber);
        }
    }
}