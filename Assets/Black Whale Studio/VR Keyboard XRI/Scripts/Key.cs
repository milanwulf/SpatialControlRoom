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

using UnityEngine;
using UnityEngine.UI;

namespace Keyboard
{
    public class Key : MonoBehaviour
    {
        [SerializeField] protected KeyChannel keyChannel;
        [SerializeField] protected KeyboardManager keyboard;
        protected Button button;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnPress);
            keyboard.onKeyboardModeChanged.AddListener(UpdateKey);
            keyChannel.onFirstKeyPress.AddListener(UpdateKey);
            keyChannel.OnKeyColorsChanged += ChangeKeyColors; 
            keyChannel.OnKeysStateChange += ChangeKeyState;
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveListener(OnPress);
            keyboard.onKeyboardModeChanged.RemoveListener(UpdateKey);
            keyChannel.onFirstKeyPress.RemoveListener(UpdateKey);
            keyChannel.OnKeyColorsChanged -= ChangeKeyColors;
            keyChannel.OnKeysStateChange -= ChangeKeyState;
        }

        protected virtual void OnPress()
        {
            keyboard.DeactivateShift();
        }

        protected virtual void UpdateKey()
        {
            // empty method for override in child classes
        }
        
        protected void ChangeKeyColors(Color normalColor, Color highlightedColor, Color pressedColor, Color selectedColor)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = normalColor;
            cb.highlightedColor = highlightedColor;
            cb.pressedColor = pressedColor;
            cb.selectedColor = selectedColor;
            button.colors = cb;
        }

        protected void ChangeKeyState(bool enabled)
        {
            button.interactable = enabled;
        }
    }
}