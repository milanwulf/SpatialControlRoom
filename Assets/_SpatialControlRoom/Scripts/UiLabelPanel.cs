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
    }

    private void OnEnable()
    {
        inputField.onSelect.AddListener(OnLabelInputFieldSelect);
        lockBtn.onClick.AddListener(ToggleLockState);
        deleteBtn.onClick.AddListener(DeleteLabelAfterDelay);
    }

    private void OnDisable()
    {
        inputField.onSelect.RemoveListener(OnLabelInputFieldSelect);
        lockBtn.onClick.RemoveListener(ToggleLockState);
        keyboardSpawner.DestroyKeyboardImmediate();
        deleteBtn.onClick.RemoveListener(DeleteLabelAfterDelay);
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
        }
        mainFlexalonObject.ForceUpdate();
    }

    private void DeleteLabelAfterDelay()
    {
        Invoke(nameof(DeleteLabel), 0.3f);
    }

    private void DeleteLabel()
    {
        uiLabelManager.RemoveLabelInstance(gameObject);
    }
}
