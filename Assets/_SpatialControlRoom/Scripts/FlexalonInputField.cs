using Flexalon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlexalonInputField : MonoBehaviour
{
    private TMP_InputField _inputField;
    private FlexalonNode _textNode;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _textNode = Flexalon.Flexalon.GetOrCreateNode(_inputField.textComponent.gameObject);
        _inputField.onValueChanged.AddListener(OnValueChanged);
        Invoke("FixCaret", 0f);
    }

    private void OnValueChanged(string value)
    {
        _textNode.MarkDirty();
    }

    private void FixCaret()
    {
        var caret = _inputField.textComponent.transform.parent.Find("Caret");
        if (caret)
        {
            var fxObj = caret.gameObject.AddComponent<FlexalonObject>();
            fxObj.SkipLayout = true;
        }
        else
        {
            UnityEngine.Debug.LogWarning("Caret not found");
        }
    }
}