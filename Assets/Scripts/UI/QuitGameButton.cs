using UnityEngine;

namespace SIVS
{
    public class QuitGameButton : MonoBehaviour
    {
        public void Press() => Application.Quit();
    }
}