#if UNITY_TMPRO

using TMPro;
using UnityEngine;

namespace Flexalon.Templates
{
    // Implements DataBinding by binding a string to a TMP_Text.
    [DisallowMultipleComponent, AddComponentMenu("Flexalon Templates/Text Data Binding")]
    public class TextDataBinding : MonoBehaviour, DataBinding
    {
        private TMP_Text _text;

        void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void SetData(object data)
        {
            _text.text = (string) data;
        }
    }
}

#endif