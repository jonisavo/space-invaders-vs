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
        private const string playerNamePrefKey = "PlayerName";

        private void Start()
        {
            string defaultName = string.Empty;
            InputField inputField = this.GetComponent<InputField>();
            if (inputField != null)
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    inputField.text = defaultName;
                }

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player name is null or empty.");
                return;
            }

            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
    }
}
