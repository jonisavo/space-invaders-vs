using System.Collections;
using System.Collections.Generic;
using RedBlueGames.NotNull;
using RedBlueGames.Tools.TextTyper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Animator))]
    public class MainMenuButton : RainbowAnimationImage
    {
        [Header("Pulsing")]
        [Tooltip("The sprite used for generated pulses.")]
        public Sprite pulseSprite;
        
        [Range(0.05f, 2f)]
        [Tooltip("The speed of the pulse effect.")]
        public float pulseSpeed = 0.7f;

        [Range(1.0f, 2f)]
        [Tooltip("The scale multiplier of the pulse at the beginning.")]
        public float pulseStartMultiplier = 1.0f;

        [Range(1.1f, 3f)]
        [Tooltip("The scale multiplier of the pulse at the end.")]
        public float pulseEndMultiplier = 1.2f;

        [Range(0.05f, 2f)]
        [Tooltip("The interval of individual pulse spawns.")]
        public float pulseSpawnInterval = 0.2f;

        [Range(2f, 6f)]
        [Tooltip("The time between pulses.")]
        public float timeBetweenPulses = 2f;
        
        [FormerlySerializedAs("additionalElements")]
        [Header("Additional Elements to Update")]
        [Tooltip("Additional rainbow gradient effects to apply.")]
        [NotNull]
        public RainbowAnimationMonoBehaviour[] additionalRainbowAnimation;

        [Tooltip("Additional TextAnimators to animate.")]
        [NotNull]
        public TextAnimator[] additionalTextAnimators;

        private bool _selected;

        private readonly List<GameObject> _pulseObjects = new List<GameObject>();
        
        private Coroutine _pulseCoroutine;

        private const uint EffectObjectCount = 3;

        private EventSystem _eventSystem;

        protected override void Awake()
        {
            base.Awake();
            
            _eventSystem = EventSystem.current;

            for (var i = 0; i < EffectObjectCount; i++)
                InstantiatePulseObject("Pulse Object " + i);
        }

        private void InstantiatePulseObject(string objName)
        {
            var obj = new GameObject(objName, typeof(Image), typeof(RainbowAnimationImage), typeof(CanvasGroup));

            var image = obj.GetComponent<Image>();

            if (pulseSprite)
                image.sprite = pulseSprite;
            else
                image.sprite = _image.sprite;

            var rainbowAnimation = obj.GetComponent<RainbowAnimationImage>();

            rainbowAnimation.animationSpeed = animationSpeed;
            
            rainbowAnimation.EnableAllAnimation();

            var canvasGroup = obj.GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;

            canvasGroup.interactable = false;

            canvasGroup.blocksRaycasts = false;

            var rectTransform = obj.GetComponent<RectTransform>();

            rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
            
            obj.transform.SetParent(gameObject.transform, false);

            obj.transform.Translate(0, 0, -1);
            
            _pulseObjects.Add(obj);
        }

        protected override void Update()
        {
            if (_selected || _active)
                UpdateColor();
        }

        public override void UpdateColor()
        {
            base.UpdateColor();

            foreach (var additionalElement in additionalRainbowAnimation)
                additionalElement.UpdateColor();
        }

        public void HandlePointerEnter(BaseEventData evt)
        {
            if (!_active && !_selected)
                EnableAllAnimation();
        }

        public void HandlePointerExit(BaseEventData evt)
        {
            if (_active && !_selected)
                DisableAllAnimation();
        }

        public void HandleSelect(BaseEventData evt)
        {
            _selected = true;
            
            if (!_active)
                EnableAllAnimation();
        }

        public void HandleDeselect(BaseEventData evt)
        {
            _selected = false;
            
            if (_active)
                DisableAllAnimation();
        }

        public override void EnableAllAnimation()
        {
            base.EnableAllAnimation();

            foreach (var rainbowAnimation in additionalRainbowAnimation)
                rainbowAnimation.EnableAllAnimation();
            
            foreach (var textAnimator in additionalTextAnimators)
                textAnimator.StartAnimation();
            
            if (_pulseCoroutine != null)
                StopCoroutine(_pulseCoroutine);

            _pulseCoroutine = StartCoroutine(PulseCoroutine());
        }

        public override void DisableAllAnimation()
        {
            base.DisableAllAnimation();

            foreach (var rainbowAnimation in additionalRainbowAnimation)
                rainbowAnimation.DisableAllAnimation();
            
            foreach (var textAnimator in additionalTextAnimators)
                textAnimator.StopAnimation();

            if (_pulseCoroutine == null)
                return;
            
            StopCoroutine(_pulseCoroutine);

            _pulseCoroutine = null;
        }

        private IEnumerator PulseCoroutine()
        {
            var individualSpawnInterval = new WaitForSeconds(pulseSpawnInterval);

            var waitBetweenSpawns = new WaitForSeconds(timeBetweenPulses);

            while (true)
            {
                yield return waitBetweenSpawns;

                if (_eventSystem.currentSelectedGameObject != null &&
                    _eventSystem.currentSelectedGameObject != gameObject)
                    continue;
                
                foreach (var obj in _pulseObjects)
                {
                    StartCoroutine(MovePulseObjectCoroutine(obj));

                    yield return individualSpawnInterval;
                }
            }
        }

        private IEnumerator MovePulseObjectCoroutine(GameObject obj)
        {
            var objCanvasGroup = obj.GetComponent<CanvasGroup>();
            var objTransform = obj.transform;
            
            obj.GetComponent<RainbowAnimationImage>().animationSpeed = animationSpeed;

            var startScale = Vector3.one * pulseStartMultiplier;
            var endScale = Vector3.one * pulseEndMultiplier;
            
            objTransform.localScale = startScale;
            objCanvasGroup.alpha = 1f;

            while (objCanvasGroup.alpha > 0)
            {
                objCanvasGroup.alpha -= pulseSpeed * Time.deltaTime;
                
                objTransform.localScale = Vector3.Lerp(endScale, startScale, objCanvasGroup.alpha);

                yield return null;
            }
        }
    }
}