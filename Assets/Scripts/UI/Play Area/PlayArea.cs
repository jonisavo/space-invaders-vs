using UnityEngine;

namespace SIVS
{
    public class PlayArea : MonoBehaviour
    {
        [Range(0f, 8f)]
        public float throwForce = 1f;

        [Range(0f, 5f)]
        public float throwTorque = 1f;
        
        public SIVSPlayer Player { get; private set; }

        public delegate void InitializeDelegate(SIVSPlayer player);

        public event InitializeDelegate OnInitialize;

        private void OnEnable() => SIVSPlayer.OnLivesChange += HandleLivesChange;

        private void OnDisable() => SIVSPlayer.OnLivesChange -= HandleLivesChange;

        public void Initialize(SIVSPlayer player)
        {
            Player = player;

            OnInitialize?.Invoke(player);
        }

        private void HandleLivesChange(SIVSPlayer player, int newLives)
        {
            if (player.Number != Player.Number || newLives > 0)
                return;
            
            ThrowAside();
        }
        
        private void ThrowAside()
        {
            var torque = Player.Number == 1 ? -throwTorque : throwTorque;

            foreach (var childRb in GetComponentsInChildren<Rigidbody2D>())
                ThrowRigidbody(childRb, torque);
        }

        private void ThrowRigidbody(Rigidbody2D rb, float torque)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            var currentPosition = gameObject.transform.position + Vector3.down * 1.5f;

            var force = (rb.transform.position - currentPosition).normalized * throwForce;
            
            rb.AddForceAtPosition(force, currentPosition, ForceMode2D.Impulse);
            
            rb.AddTorque(torque, ForceMode2D.Impulse);
        }
    }
}