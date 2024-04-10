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
using UnityEngine.Events;

// [CreateAssetMenu(fileName = "KeyChannel", menuName = "Channels/KeyChannel")]
namespace Keyboard
{
    public class KeyChannel : ScriptableObject
    {
        public UnityAction<string> OnKeyPressed;
        public UnityAction<Color, Color, Color, Color> OnKeyColorsChanged;
        public UnityAction<bool> OnKeysStateChange;
        public UnityEvent onFirstKeyPress;

        public void RaiseKeyPressedEvent(string key) =>
            OnKeyPressed?.Invoke(key);

        public void RaiseKeyColorsChangedEvent(Color normalColor, Color highlightedColor, Color pressedColor,
            Color selectedColor) =>
            OnKeyColorsChanged?.Invoke(normalColor, highlightedColor, pressedColor, selectedColor);

        public void RaiseKeysStateChangeEvent(bool enabled) =>
            OnKeysStateChange?.Invoke(enabled);
    }
}