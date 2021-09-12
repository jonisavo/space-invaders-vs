using TMPro;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionNumberText : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_Text>().text = Application.version;
        }
    }
}
