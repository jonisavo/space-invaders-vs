using UnityEngine;

namespace SIVS
{
    public class InvaderHealth : MonoBehaviour
    {
        [SerializeField]
        private int health;

        private GameStatistics statistics;

        private GameRandomizer randomizer;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            statistics = GameObject.Find("Game Manager").GetComponent<GameStatistics>();
            randomizer = GameObject.Find("Game Manager").GetComponent<GameRandomizer>();
            // if (statistics.TotalInvaderKills() > 100)
            //     health = randomizer.GetInt(5, 11);
            // else if (statistics.TotalInvaderKills() > 50)
            //     health = randomizer.GetInt(3, 7);
            // else if (statistics.TotalInvaderKills() > 15)
            //     health = randomizer.GetInt(1, 4);
            // else
            //     health = 1;

            health = randomizer.GetInt(1, 11);
            
            TintSprite();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet")) return;
            Destroy(other.gameObject);
            LoseHealth();
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
            float hue = 1.0f - (health - 1) * 0.1f;
            spriteRenderer.color = new Color(1.0f, hue, hue);
        }
    }   
}
