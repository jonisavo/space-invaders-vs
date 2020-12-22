using UnityEngine;

namespace SIVS
{
    public class UIManager : MonoBehaviour
    {
        public Texture lifeTexture;

        private GameStatistics _statistics;

        private void Start()
        {
            _statistics = GameObject.Find("Game Manager").GetComponent<GameStatistics>();
        }

        private void OnGUI()
        {
            DrawLives();
        }

        private void DrawLives()
        {
            for (var i = 0; i < _statistics.GetOwnStatistics().Lives; i++)
                GUI.DrawTexture(new Rect(12 + 40 + 40 * i, 656, 32, 32), lifeTexture);

            for (var i = 0; i < _statistics.GetOpponentStatistics().Lives; i++)
                GUI.DrawTexture(new Rect(1024 - 36 - 40 - 40 * i, 656, 32, 32), lifeTexture);
        }
    }
}
