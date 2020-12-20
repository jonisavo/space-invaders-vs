using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace SIVS
{
    public class GameRandomizer : MonoBehaviour
    {
        private Random _rand;

        private void Awake()
        {
            _rand = new Random(PhotonNetwork.CurrentRoom.GetHashCode());
        }

        public int GetInt(int minValue, int maxValue)
        {
            return _rand.Next(minValue, maxValue);
        }
    }
}

