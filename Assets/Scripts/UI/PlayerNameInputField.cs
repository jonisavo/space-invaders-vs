using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using TMPro;

namespace SIVS
{
    /// <summary>
    /// Player name input field. Let the user input his name, will appear above the player in the game.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        private const string PlayerNamePrefKey = "PlayerName";

        private void Start()
        {
            var defaultName = string.Empty;
            var inputField = this.GetComponent<TMP_InputField>();

            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                inputField.text = defaultName;
            }

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(PlayerNamePrefKey, value);
        }
    }
}
