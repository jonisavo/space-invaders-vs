using UnityEngine;

namespace SIVS
{
    public class PlayArea : MonoBehaviour
    {
        public SIVSPlayer Player { get; private set; }

        public delegate void OnInitializeDelegate(SIVSPlayer player);

        public event OnInitializeDelegate OnInitialize;

        public void Initialize(SIVSPlayer player)
        {
            Player = player;

            OnInitialize?.Invoke(player);
        }
    }
}