using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class Powerup : MonoBehaviourPun
    {
        [Tooltip("Sound effect played when the powerup is obtained.")]
        public AudioClip soundEffect;

        [Tooltip("GameObject with a TextPopup component to instantiate when the powerup is obtained.")]
        public GameObject popupObject;

        [TextArea]
        [Tooltip("Text for the popup. Leave blank to use the component's own text field.")]
        public string popupText;

        public delegate void OnGetDelegate(int actorNumber);

        public static event OnGetDelegate OnGet;

        private TextPopup _textPopup;

        private void Awake() => _textPopup = popupObject.GetComponent<TextPopup>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            var player = other.gameObject.GetPhotonView().Owner;

            if (PhotonNetwork.LocalPlayer.ActorNumber != player.ActorNumber)
                return;
            
            OnPowerupGet(other.gameObject, player.toSIVSPlayer());
            
            photonView.RPC(nameof(ObtainPowerup), RpcTarget.All, player.ActorNumber);
        }

        protected virtual void OnPowerupGet(GameObject obj, SIVSPlayer player)
        {
            SoundPlayer.PlaySound(soundEffect);
        }

        [PunRPC]
        protected void ObtainPowerup(int actorNumber)
        {
            OnGet?.Invoke(actorNumber);
            
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

        [PunRPC]
        public void DestroyPowerupRPC() => DestroyPowerup();

        private void DestroyPowerup()
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
            else
                if (gameObject) gameObject.SetActive(false);
        }

        protected void ChangeTextPopup(GameObject newPopupObject, string newText)
        {
            popupObject = newPopupObject;
            popupText = newText;
        }
    }
}