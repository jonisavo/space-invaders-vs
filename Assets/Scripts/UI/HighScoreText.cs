using TMPro;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Text))]
    public class HighScoreText : MonoBehaviour
    {
        private void Awake()
        {
            var highScore = PlayerPrefs.GetInt("HighScore", 0);

            GetComponent<TMP_Text>().text = highScore.ToString("D6");
        }
    }
}
