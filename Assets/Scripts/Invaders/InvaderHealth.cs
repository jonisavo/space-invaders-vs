using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InvaderHealth : MonoBehaviour
    {
        [SerializeField]
        private int health;

        private GameStatistics _statistics;

        private GameRandomizer _randomizer;

        private SpriteRenderer _spriteRenderer;
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _statistics = GameObject.Find("Game Manager").GetComponent<GameStatistics>();
            _randomizer = GameObject.Find("Game Manager").GetComponent<GameRandomizer>();
            InitializeHealth();
            TintSprite();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet")) return;
            Destroy(other.gameObject);
            LoseHealth();
        }
        
        #endregion

        private void InitializeHealth()
        {
            if (_statistics.TotalInvaderKills > 100)
                health = _randomizer.GetInt(5, 11);
            else if (_statistics.TotalInvaderKills > 50)
                health = _randomizer.GetInt(3, 7);
            else if (_statistics.TotalInvaderKills > 15)
                health = _randomizer.GetInt(1, 4);
            else
                health = 1;
        }

        private void LoseHealth()
        {
            health--;
            if (health <= 0)
                Die();
            else
                TintSprite();
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void TintSprite()
        {
            var hue = 1.0f - (health - 1) * 0.1f;
            _spriteRenderer.color = new Color(1.0f, hue, hue);
        }
    }   
}
