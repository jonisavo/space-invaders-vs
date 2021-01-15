using UnityEngine;
using Random = System.Random;

namespace SIVS
{
    [RequireComponent(typeof(Animator))]
    public class InvaderSprite : MonoBehaviour
    {
        [Tooltip("Specifies the number of sprite types to cycle through.")]
        public int typeCount;

        private static readonly int Type = Animator.StringToHash("Type");

        private void Awake()
        {
            var rand = new Random(System.Guid.NewGuid().GetHashCode());
            GetComponent<Animator>().SetInteger(Type, rand.Next(typeCount));
        }
    }
}
