using UnityEngine;

namespace SIVS
{
    public class PlatformMonoBehaviour : MonoBehaviour
    {
        public bool standalone;

        public bool playStation4;
        
#if UNITY_STANDALONE || UNITY_PS4
        private void Awake()
        {
#if UNITY_STANDALONE
            if (standalone)
#elif UNITY_PS4
            if (playStation4)
#endif
                OnAwake();
        }
#endif

        protected virtual void OnAwake() {}
    }
}