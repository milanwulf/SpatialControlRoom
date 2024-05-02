using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Flexalon;
using UnityEngine.UI;
using Google.MaterialDesign.Icons;

public class UiLabelPanel : MonoBehaviour
{
    [SerializeField] private KeyboardSpawner keyboardSpawner;
    [SerializeField] private GameObject inputFieldObj;
    private TMP_InputField inputField;
    private Image inputFieldStroke;
    private bool uiIsLocked = false;
    private FlexalonObject mainFlexalonObject;
    private UiLabelManager uiLabelManager;

    private Color caretColor;

    [Header("Buttons")]
    [SerializeField] private Button lockBtn;
    [SerializeField] private MaterialIcon lockBtnIcon;
    private string unlockIconUnicode = "e898";
    private string lockIconUnicode = "e897";

    [SerializeField] private GameObject deleteBtnObj;
    private Button deleteBtn;

    [Header("Handles")]
    [SerializeField] private GameObject bottomHandle;

    private void Awake()
    {
        mainFlexalonObject = GetComponent<FlexalonObject>();
        uiLabelManager = FindObjectOfType<UiLabelManager>();
        deleteBtn = deleteBtnObj.GetComponent<Button>();
        inputField = inputFieldObj.GetComponent<TMP_InputField>();
        inputFieldStroke = inputFieldObj.GetComponent<Image>();
        caretColor = inputField.caretColor;
    }

    private void OnEnable()
    {
        inputField.onSelect.AddListener(OnLabelInputFieldSelect);
        lockBtn.onClick.AddListener(ToggleLockState);
        deleteBtn.onClick.AddListener(DeleteLabel);
    }

    private void OnDisable()
    {
        inputField.onSelect.RemoveListener(OnLabelInputFieldSelect);
        lockBtn.onClick.RemoveListener(ToggleLockState);
        keyboardSpawner.DestroyKeyboardImmediate();
        deleteBtn.onClick.RemoveListener(DeleteLabel);
    }

    private void OnLabelInputFieldSelect(string label)
    {
        keyboardSpawner.InstantiateKeyboard(inputField);
    }

    private void ToggleLockState()
    {
        uiIsLocked = !uiIsLocked;
        inputField.interactable = !uiIsLocked;
        inputFieldStroke.enabled = !uiIsLocked;
        bottomHandle.SetActive(!uiIsLocked);
        deleteBtnObj.GetComponent<FlexalonObject>().SkipLayout = uiIsLocked;
        deleteBtnObj.SetActive(!uiIsLocked);
        lockBtnIcon.iconUnicode = uiIsLocked ? lockIconUnicode : unlockIconUnicode;
        if (uiIsLocked)
        {
            inputField.DeactivateInputField();
            keyboardSpawner.DestroyKeyboardImmediate();
            caretColor = new Color(caretColor.r, caretColor.g, caretColor.b, 0);
        }

        else
        {
            caretColor = new Color(caretColor.r, caretColor.g, caretColor.b, 1);
        }
        inputField.caretColor = caretColor;
        mainFlexalonObject.ForceUpdate();
    }

    private void DeleteLabel()
    {
        gameObject.SetActive(false);
        Invoke(nameof(RemoveInstance), 0.5f); //delay is needed otherwise Oculus Interaction will throw an null reference error
    }

    private void RemoveInstance()
    {
        uiLabelManager.RemoveLabelInstance(gameObject);
    }
}
