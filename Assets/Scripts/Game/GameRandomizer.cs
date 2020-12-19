using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace SIVS
{
    public class GameRandomizer : MonoBehaviour
    {
        private Random rand;

        private void Awake()
        {
            rand = new Random(PhotonNetwork.CurrentRoom.GetHashCode());
        }

        public int GetInt(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }
    }
}

