﻿using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
#endif
using TMPro;
using UnityEngine;

namespace SIVS
{
    public class ResolutionDropdown : MainMenuDropdown
    {
        private List<Resolution> _supportedResolutions = new List<Resolution>();

        protected override void Awake()
        {
            _supportedResolutions = GetSupportedResolutions();

            base.Awake();
            
            Dropdown.onValueChanged.AddListener(HandleOptionChange);
        }

        private void Start() => HandleOptionChange(Dropdown.value);

        private void HandleOptionChange(int i)
        {
            var resolution = _supportedResolutions[i];

            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            
            PlayerPrefs.SetInt(PlayerPrefsKeys.ScreenResolution, i);
        }

        protected override InitialDropdownValues GetInitialValues()
        {
            var optionsList = new List<TMP_Dropdown.OptionData>();

            var index = 0;

            var currentResolution = Screen.currentResolution;

            for (var i = 0; i < _supportedResolutions.Count; i++)
            {
                var resolution = _supportedResolutions[i];
                
                optionsList.Add(new TMP_Dropdown.OptionData($"{resolution.width}x{resolution.height}"));

                if (resolution.width == currentResolution.width && resolution.height == currentResolution.height)
                    index = i;
            }

            var prefIndex = PlayerPrefs.GetInt(PlayerPrefsKeys.ScreenResolution, index);

            index = Mathf.Clamp(prefIndex, 0, _supportedResolutions.Count - 1);

            return new InitialDropdownValues
            {
                Options = optionsList,
                Index = index
            };
        }

        private static List<Resolution> GetSupportedResolutions()
        {
#if UNITY_EDITOR
            return Screen.resolutions.ToList();
#else
            var resolutionList = new List<Resolution>();

            foreach (var resolution in Screen.resolutions)
                if (resolution.refreshRate == 60)
                    resolutionList.Add(resolution);

            return resolutionList;
#endif
        }
    }
}