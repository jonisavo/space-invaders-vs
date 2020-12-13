using UnityEngine;
using Random = System.Random;

namespace SIVS
{
    [RequireComponent(typeof(Animator))]
    public class InvaderSprite : MonoBehaviour
    {
        public int TypeCount;

        private void Awake()
        {
            var rand = new Random(System.Guid.NewGuid().GetHashCode());
            GetComponent<Animator>().SetInteger("Type", rand.Next(TypeCount));
        }
    }
}