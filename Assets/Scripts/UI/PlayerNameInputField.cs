using UnityEngine;

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
        public GameObject warningLabel;
        
        private const string PlayerNamePrefKey = "PlayerName";

        private void Start()
        {
            var defaultName = string.Empty;
            var inputField = GetComponent<TMP_InputField>();

            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                inputField.text = defaultName;
            }

            PhotonNetwork.NickName = defaultName;
            SetWarningVisibility(string.IsNullOrEmpty(defaultName.Trim()));
        }

        public void SetPlayerValue(string value)
        {
            PhotonNetwork.NickName = value;
            
            if (string.IsNullOrEmpty(value.Trim()))
            {
                SetWarningVisibility(true);
                return;
            }
            
            PlayerPrefs.SetString(PlayerNamePrefKey, value);
            
            SetWarningVisibility(false);
        }

        private void SetWarningVisibility(bool visible) => warningLabel.SetActive(visible);
    }
}
