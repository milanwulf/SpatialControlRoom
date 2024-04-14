using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiLabelPanel : MonoBehaviour
{
    [SerializeField] private KeyboardSpawner keyboardSpawner;
    private TMP_InputField labelInputField;

    private void OnEnable()
    {
        labelInputField = GetComponentInChildren<TMP_InputField>();
        labelInputField.onSelect.AddListener(OnLabelInputFieldSelect);
    }

    private void OnDisable()
    {
        labelInputField.onSelect.RemoveListener(OnLabelInputFieldSelect);
    }

    private void OnLabelInputFieldSelect(string label)
    {
        keyboardSpawner.InstantiateKeyboard(labelInputField);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
