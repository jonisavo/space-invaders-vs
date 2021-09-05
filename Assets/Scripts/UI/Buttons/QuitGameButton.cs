using UnityEngine;

namespace SIVS
{
    public class QuitGameButton : MonoBehaviour
    {
        public void Press()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}