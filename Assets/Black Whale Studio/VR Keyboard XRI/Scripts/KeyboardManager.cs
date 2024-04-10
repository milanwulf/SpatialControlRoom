/*
 * Copyright (c) 2023 Black Whale Studio. All rights reserved.
 *
 * This software is the intellectual property of Black Whale Studio. Direct use, copying, or distribution of this code in its original or only slightly modified form is strictly prohibited. Significant modifications or derivations are required for any use.
 *
 * If this code is intended to be used in a commercial setting, you must contact Black Whale Studio for explicit permission.
 *
 * For the full licensing terms and conditions, visit:
 * https://blackwhale.dev/
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT ANY WARRANTIES OR CONDITIONS.
 *
 * For questions or to join our community, please visit our Discord: https://discord.gg/55gtTryfWw
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        [Header("Keyboard Setup")]
        [SerializeField] private KeyChannel keyChannel;
        [SerializeField] private Button spacebarButton;
        [SerializeField] private Button speechButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button switchButton;
        [SerializeField] private string switchToNumbers = "Numbers";
        [SerializeField] private string switchToLetter = "Letters";

        private TextMeshProUGUI switchButtonText;
        
        [Header("Keyboards")]
        [SerializeField] private GameObject lettersKeyboard;
        [SerializeField] private GameObject numbersKeyboard;
        [SerializeField] private GameObject specialCharactersKeyboard;

        [Header("Shift/Caps Lock Button")] 
        [SerializeField] internal bool autoCapsAtStart = true;
        [SerializeField] private Button shiftButton;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite activeSprite;
        
        [Header("Switch Number/Special Button")]
        [SerializeField] private Button switchNumberSpecialButton;
        [SerializeField] private string numbersString = "Numbers";
        [SerializeField] private string specialString = "Special";

        private TextMeshProUGUI switchNumSpecButtonText;
        
        [Header("Keyboard Button Colors")]
        [SerializeField] private Color normalColor = Color.black;
        [SerializeField] private Color highlightedColor = Color.yellow;
        [SerializeField] private Color pressedColor = Color.red;
        [SerializeField] private Color selectedColor = Color.blue;
        
        [Header("Output Field Settings")]
        [SerializeField] private TMP_InputField outputField;
        [SerializeField] private Button enterButton;
        [SerializeField] private int maxCharacters = 15;
        [SerializeField] private int minCharacters = 3;

        private ColorBlock shiftButtonColors;
        private bool isFirstKeyPress = true;
        private bool keyHasBeenPressed;
        private bool shiftActive;
        private bool capsLockActive;
        private bool specialCharactersActive;
        private float lastShiftClickTime;
        private float shiftDoubleClickDelay = 0.5f;

        public UnityEvent onKeyboardModeChanged;

        private void Awake()
        {
            shiftButtonColors = shiftButton.colors;
            
            CheckTextLength();

            speechButton.interactable = false;
            
            numbersKeyboard.SetActive(false);
            specialCharactersKeyboard.SetActive(false);
            lettersKeyboard.SetActive(true);

            spacebarButton.onClick.AddListener(OnSpacePress);
            deleteButton.onClick.AddListener(OnDeletePress);
            switchButton.onClick.AddListener(OnSwitchPress);
            shiftButton.onClick.AddListener(OnShiftPress);
            switchNumberSpecialButton.onClick.AddListener(SwitchBetweenNumbersAndSpecialCharacters);
            switchButtonText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
            switchNumSpecButtonText = switchNumberSpecialButton.GetComponentInChildren<TextMeshProUGUI>();
            keyChannel.RaiseKeyColorsChangedEvent(normalColor, highlightedColor, pressedColor, selectedColor);
            
            switchNumberSpecialButton.gameObject.SetActive(false);
            numbersKeyboard.SetActive(false);
            specialCharactersKeyboard.SetActive(false);

            if (!autoCapsAtStart) return;
            ActivateShift();
            UpdateShiftButtonAppearance();
        }

        public TMP_InputField CurrentInputField { get; private set; }

        public void SetInputField(TMP_InputField inputField) //added this method
        {
            CurrentInputField = inputField;
            outputField = inputField;
        }

        private void OnDestroy()
        {
            spacebarButton.onClick.RemoveListener(OnSpacePress);
            deleteButton.onClick.RemoveListener(OnDeletePress);
            switchButton.onClick.RemoveListener(OnSwitchPress);
            shiftButton.onClick.RemoveListener(OnShiftPress);
            switchNumberSpecialButton.onClick.RemoveListener(SwitchBetweenNumbersAndSpecialCharacters);
        }

        private void OnEnable() => keyChannel.OnKeyPressed += KeyPress;

        private void OnDisable() => keyChannel.OnKeyPressed -= KeyPress;

        private void KeyPress(string key)
        {
            keyHasBeenPressed = true;
            bool wasShiftActive = shiftActive;
            DeactivateShift();

            string textToInsert;
            if (wasShiftActive || capsLockActive)
            {
                textToInsert = key.ToUpper();
            }
            else
            {
                textToInsert = key.ToLower();
            }

            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            outputField.text = outputField.text.Remove(startPos, endPos - startPos);
            outputField.text = outputField.text.Insert(startPos, textToInsert);

            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos + textToInsert.Length;

            if (isFirstKeyPress)
            {
                isFirstKeyPress = false;
                keyChannel.onFirstKeyPress.Invoke();
            }
    
            CheckTextLength();
        }

        private void OnSpacePress()
        {
            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            outputField.text = outputField.text.Remove(startPos, endPos - startPos);
            outputField.text = outputField.text.Insert(startPos, " ");

            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos + 1;
            
            CheckTextLength();
        }

        private void OnDeletePress()
        {
            if (string.IsNullOrEmpty(outputField.text)) return;
            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            if (endPos > startPos)
            {
                outputField.text = outputField.text.Remove(startPos, endPos - startPos);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos;
            }
            else if (startPos > 0)
            {
                outputField.text = outputField.text.Remove(startPos - 1, 1);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos - 1;
            }
            
            CheckTextLength();
        }

        private void CheckTextLength()
        {
            int currentLength = outputField.text.Length;

            // Raise event to enable or disable keys based on the text length
            bool keysEnabled = currentLength < maxCharacters;
            keyChannel.RaiseKeysStateChangeEvent(keysEnabled);

            // Disables or enables the enter button based on the text length
            enterButton.interactable = currentLength >= minCharacters;

            // Always enable the delete button, regardless of the text length
            deleteButton.interactable = true;
            
            // Disable shift/caps lock if maximum text length is reached
            if (currentLength != maxCharacters) return;
            DeactivateShift();
            capsLockActive = false;
            UpdateShiftButtonAppearance();
        }

        private void OnSwitchPress()
        {
            if (lettersKeyboard.activeSelf)
            {
                lettersKeyboard.SetActive(false);
                numbersKeyboard.SetActive(true);
                specialCharactersKeyboard.SetActive(false);
                switchNumberSpecialButton.gameObject.SetActive(true);

                // Set buttons' text
                switchButtonText.text = switchToNumbers;
                switchNumSpecButtonText.text = specialString;
            }
            else
            {
                lettersKeyboard.SetActive(true);
                numbersKeyboard.SetActive(false);
                specialCharactersKeyboard.SetActive(false);
                switchNumberSpecialButton.gameObject.SetActive(false);

                // Set buttons' text
                switchButtonText.text = switchToLetter;
                switchNumSpecButtonText.text = specialString;
            }
            DeactivateShift();
            onKeyboardModeChanged?.Invoke();
        }


        private void OnShiftPress()
        {
            if (capsLockActive)
            {
                // If Caps Lock is active, deactivate it
                capsLockActive = false;
                shiftActive = false;
            }
            else switch (shiftActive)
            {
                case true when !keyHasBeenPressed && Time.time - lastShiftClickTime < shiftDoubleClickDelay:
                    // If Shift is active, a key has not been pressed, and Shift button was double clicked, activate Caps Lock
                    capsLockActive = true;
                    shiftActive = false;
                    break;
                case true when !keyHasBeenPressed:
                    // If Shift is active, a key has not been pressed, deactivate Shift
                    shiftActive = false;
                    break;
                case false:
                    // If Shift is not active and Shift button was clicked once, activate Shift
                    shiftActive = true;
                    break;
            }

            lastShiftClickTime = Time.time;
            UpdateShiftButtonAppearance();
            onKeyboardModeChanged?.Invoke();
        }

        private void ActivateShift()
        {
            if (!capsLockActive) shiftActive = true;

            UpdateShiftButtonAppearance();
            onKeyboardModeChanged?.Invoke();
        }

        public void DeactivateShift()
        {
            if (shiftActive && !capsLockActive && keyHasBeenPressed)
            {
                shiftActive = false;
                UpdateShiftButtonAppearance();
                onKeyboardModeChanged?.Invoke();
            }

            keyHasBeenPressed = false;
        }

        public bool IsShiftActive() => shiftActive;

        public bool IsCapsLockActive() => capsLockActive;

        private void SwitchBetweenNumbersAndSpecialCharacters()
        {
            if (lettersKeyboard.activeSelf) return;

            // Switch between numbers and special characters keyboard
            bool isNumbersKeyboardActive = numbersKeyboard.activeSelf;
            numbersKeyboard.SetActive(!isNumbersKeyboardActive);
            specialCharactersKeyboard.SetActive(isNumbersKeyboardActive);

            switchNumSpecButtonText.text = switchNumSpecButtonText.text == specialString ? numbersString : specialString;

            onKeyboardModeChanged?.Invoke();
        }

        private void UpdateShiftButtonAppearance()
        {
            if (capsLockActive)
            {
                shiftButtonColors.normalColor = highlightedColor;
                buttonImage.sprite = activeSprite;
            }
            else if(shiftActive)
            {
                shiftButtonColors.normalColor = highlightedColor;
                buttonImage.sprite = defaultSprite;
            }
            else
            {
                shiftButtonColors.normalColor = normalColor;
                buttonImage.sprite = defaultSprite;
            }

            shiftButton.colors = shiftButtonColors;
        }
    }
}