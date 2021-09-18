using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    public class JoinNamedRoomButton : MainMenuTextInputButton
    {
        [Header("Join Named Room Button")]
        [Tooltip("The Matchmaker script.")]
        [NotNull (IgnorePrefab = true)]
        public Matchmaker matchmaker;

        public override void OnTextInputConfirm(string input)
        {
            base.OnTextInputConfirm(input);
            
            if (IsInputValid(input))
                matchmaker.JoinNamedRoom(input);
        }
    }
}