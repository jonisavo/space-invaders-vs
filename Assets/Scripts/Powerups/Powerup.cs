using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    public class Powerup : MonoBehaviour
    {
        [Tooltip("Sound effect played when the powerup is obtained.")]
        [NotNull]
        public AudioClip soundEffect;

        [Tooltip("GameObject with a TextPopup component to instantiate when the powerup is obtained.")]
        [NotNull]
        public GameObject popupObject;

        [TextArea]
        [Tooltip("Text for the popup. Leave blank to use the component's own text field.")]
        public string popupText;

        public delegate void OnGetDelegate(int playerNumber);

        public static event OnGetDelegate OnGet;

        private TextPopup _textPopup;

        protected virtual void Awake() => _textPopup = popupObject.GetComponent<TextPopup>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            OnPlayerEnter(other.gameObject);
        }

        protected virtual void OnPlayerEnter(GameObject playerGameObject)
        {
            var player = GetPlayerFromObject(playerGameObject);
            
            OnPowerupGet(playerGameObject, player);
            
            ObtainPowerup(player.Number);
        }

        protected virtual SIVSPlayer GetPlayerFromObject(GameObject playerGameObject) =>
            playerGameObject.GetComponent<Ownership>().Owner;

        protected virtual void OnPowerupGet(GameObject obj, SIVSPlayer player)
        {
            SoundPlayer.PlaySound(soundEffect);
        }
        
        protected virtual void ObtainPowerup(int playerNumber)
        {
            OnGet?.Invoke(playerNumber);
            
            ShowTextPopup();
            
            DestroyPowerup();
        }

        private void ShowTextPopup()
        {
            var popupObj = Instantiate(popupObject, transform.position, Quaternion.identity);
            _textPopup = popupObj.GetComponent<TextPopup>();

            if (!string.IsNullOrEmpty(popupText))
                _textPopup.ChangeText(popupText);

            _textPopup.Show();
        }

        protected virtual void DestroyPowerup()
        {
            Destroy(gameObject);
        }

        protected void ChangeTextPopup(GameObject newPopupObject, string newText)
        {
            popupObject = newPopupObject;
            popupText = newText;
        }
    }
}