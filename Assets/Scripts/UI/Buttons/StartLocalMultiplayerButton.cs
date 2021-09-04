using UnityEngine;
using UnityEngine.SceneManagement;

namespace SIVS
{
    public class StartLocalMultiplayerButton : MonoBehaviour
    {
        public void Press() => SceneManager.LoadScene("InGameOffline");
    }
}
