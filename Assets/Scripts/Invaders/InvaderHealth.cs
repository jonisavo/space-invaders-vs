using UnityEngine;
using Random = System.Random;

namespace SIVS
{
    public class InvaderHealth : MonoBehaviour
    {
        public int health;

        private GameStatistics gameStatistics;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Random rand = new Random(System.Guid.NewGuid().GetHashCode());
            gameStatistics = GameObject.Find("Game Manager").GetComponent<GameStatistics>();
            // if (gameStatistics.TotalInvaderKills() > 100)
            //     health = rand.Next(5, 11);
            // else if (gameStatistics.TotalInvaderKills() > 50)
            //     health = rand.Next(3, 7);
            // else if (gameStatistics.TotalInvaderKills() > 15)
            //     health = rand.Next(1, 4);
            // else
            //     health = 1;

            health = rand.Next(1, 11);
            
            TintSprite();
        }

        public void LoseHealth()
        {
            health--;
            if (health <= 0)
                Die();
            else
                TintSprite();
        }

        private void Die()
        {
            
        }

        private void TintSprite()
        {
            spriteRenderer.color = new Color(
                255, 
                Mathf.LerpUnclamped(0, 255, health), 
                Mathf.LerpUnclamped(0, 255, health));
        }
    }   
}
