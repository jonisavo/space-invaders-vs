using RedBlueGames.NotNull;

namespace RedBlueGames.Tools.TextTyper
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// A bare-bones version of TextTyper for only animating text.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextAnimator : MonoBehaviour
    {
#pragma warning disable 0649 // Ignore "Field is never assigned to" warning, as it's assigned in inspector
        [SerializeField]
        [NotNull]
        [Tooltip("The library of ShakePreset animations that can be used by this component.")]
        protected ShakeLibrary shakeLibrary;

        [SerializeField]
        [NotNull]
        [Tooltip("The library of CurvePreset animations that can be used by this component.")]
        protected CurveLibrary curveLibrary;

        [SerializeField]
        [Tooltip("If set, the animations will play even if the game is paused (Time.timeScale = 0).")]
        protected bool useUnscaledTime;

        [SerializeField]
        [Tooltip("If set, the animations will start on awake.")]
        protected bool startOnAwake;

        [TextArea]
        [Tooltip("When enabled, sets this as the component's text.")]
        public string text;
#pragma warning restore 0649

        protected TextMeshProUGUI textComponent;
        protected List<TypableCharacter> charactersToType;
        protected List<TextAnimation> animations;

        private bool _enabled;

        protected TextMeshProUGUI TextComponent
        {
            get
            {
                if (this.textComponent == null)
                {
                    this.textComponent = this.GetComponent<TextMeshProUGUI>();
                }

                return this.textComponent;
            }
        }

        protected virtual void Awake()
        {
            textComponent = GetComponent<TextMeshProUGUI>();
            
            textComponent.text = TextTagParser.RemoveCustomTags(text);
            
            if (startOnAwake)
                StartAnimation();
        }

        public void ChangeText(string newText)
        {
            text = newText;

            textComponent.text = TextTagParser.RemoveCustomTags(text);
            
            if (_enabled)
                StartAnimation();
        }

        public void StartAnimation()
        {
            if (_enabled)
                ClearAnimation();
            
            ProcessTags(text);
            
            _enabled = true;

            var textInfo = this.TextComponent.textInfo;
            textInfo.ClearMeshInfo(false);
            
            this.UpdateMeshAndAnims();
        }

        public void StopAnimation()
        {
            ClearAnimation();
            
            _enabled = false;
        }

        public void ClearAnimation()
        {
            foreach (var anim in this.animations)
                Destroy(anim);
            
            this.animations.Clear();
        }

        private void UpdateMeshAndAnims()
        {
            // This must be done here rather than in each TextAnimation's OnTMProChanged
            // b/c we must cache mesh data for all animations before animating any of them

            // Update the text mesh data (which also causes all attached TextAnimations to cache the mesh data)
            this.TextComponent.ForceMeshUpdate();

            // Force animate calls on all TextAnimations because TMPro has reset the mesh to its base state
            // NOTE: This must happen immediately. Cannot wait until end of frame, or the base mesh will be rendered
            for (int i = 0; i < this.animations.Count; i++)
            {
                this.animations[i].AnimateAllChars();
            }
        }

        /// <summary>
        /// Calculates print delays for every visible character in the string.
        /// Processes delay tags, punctuation delays, and default delays
        /// Also processes shake and curve animations and spawns
        /// the appropriate TextAnimation components
        /// </summary>
        /// <param name="text">Full text string with tags</param>
        protected virtual void ProcessTags(string text)
        {
            this.charactersToType = new List<TypableCharacter>();
            this.animations = new List<TextAnimation>();
            var textAsSymbolList = TextTagParser.CreateSymbolListFromText(text);
            
            int printedCharCount = 0;
            int customTagOpenIndex = 0;
            string customTagParam = "";
            foreach (var symbol in textAsSymbolList)
            {
                // Sprite prints a character so we need to throw it out and treat it like a character
                if (symbol.IsTag && !symbol.IsReplacedWithSprite)
                {
                    if (symbol.Tag.TagType == TextTagParser.CustomTags.Anim ||
                        symbol.Tag.TagType == TextTagParser.CustomTags.Animation)
                    {
                        if (symbol.Tag.IsClosingTag)
                        {
                            // Add a TextAnimation component to process this animation
                            TextAnimation anim = null;
                            if (this.IsAnimationShake(customTagParam))
                            {
                                Debug.Log("Got shake");
                                anim = gameObject.AddComponent<ShakeAnimation>();
                                ((ShakeAnimation)anim).LoadPreset(this.shakeLibrary, customTagParam);
                            }
                            else if (this.IsAnimationCurve(customTagParam))
                            {
                                anim = gameObject.AddComponent<CurveAnimation>();
                                ((CurveAnimation)anim).LoadPreset(this.curveLibrary, customTagParam);
                            }
                            else
                            {
                                // Could not find animation. Should we error here?
                            }

                            anim.UseUnscaledTime = this.useUnscaledTime;
                            anim.SetCharsToAnimate(customTagOpenIndex, printedCharCount - 1);
                            anim.enabled = true;
                            this.animations.Add(anim);
                        }
                        else
                        {
                            customTagOpenIndex = printedCharCount - 1;
                            customTagParam = symbol.Tag.Parameter;
                        }
                    }
                    else
                    {
                        // Tag type is likely a Unity tag, but it might be something we don't know... could error if unrecognized.
                    }

                }
                else
                {
                    printedCharCount++;
                    
                    TypableCharacter characterToType = new TypableCharacter();
                    
                    if (symbol.IsTag && symbol.IsReplacedWithSprite)
                    {
                        characterToType.IsSprite = true;
                    }
                    else
                    {
                        characterToType.Char = symbol.Character;
                    }

                    this.charactersToType.Add(characterToType);
                }
            }
        }

        private bool IsAnimationShake(string animName)
        {
            return this.shakeLibrary.ContainsKey(animName);
        }

        private bool IsAnimationCurve(string animName)
        {
            return this.curveLibrary.ContainsKey(animName);
        }

        /// <summary>
        /// This class represents a printed character moment, which should correspond with a
        /// delay in the text typer. It became necessary to make this a class when I had
        /// to account for Sprite tags which are replaced by a sprite that counts as a "visble"
        /// character. These sprites would not be in the Text string stripped of tags,
        /// so this allows us to track and print them with a delay.
        /// </summary>
        protected class TypableCharacter
        {
            public char Char { get; set; }

            public float Delay { get; set; }

            public bool IsSprite { get; set; }

            public override string ToString()
            {
                return this.IsSprite ? "Sprite" : Char.ToString();
            }
        }
    }
}
