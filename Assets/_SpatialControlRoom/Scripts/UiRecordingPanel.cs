using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.MaterialDesign.Icons;
using System;
using Unity.VisualScripting;

public class UiRecordingPanel : MonoBehaviour
{
    private UiPanelSwitcher uiPanelSwitcher;
    [SerializeField] private OBSWebSocketManager obsWebSocketManager;

    //ToggleRecordingButton
    [Header("Toggle Recording Button")]
    [SerializeField] private Button recToggleBtn;
    [SerializeField] private Color inactiveToggleBtnColor;
    private Color activeToggleBtnColor;

    [SerializeField] private MaterialIcon recToggleBtnIcon;
    private string startIconUnicode = "e061";
    private string stopIconUnicode = "ef71";
    
    [SerializeField] private TextMeshProUGUI recToggleBtnText;
    [SerializeField] private string stopRecordingText = "Stop Recording";
    [SerializeField] private string startRecordingText = "Start Recording";

    //CloseButton
    [Header("Close Panel Button")]
    [SerializeField] private Button closeBtn;


    private void OnEnable()
    {
        closeBtn.onClick.AddListener(() => uiPanelSwitcher.HideAllUiPanels());
        recToggleBtn.onClick.AddListener(ToggleRecording);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(() => uiPanelSwitcher.HideAllUiPanels());
        recToggleBtn.onClick.RemoveListener(ToggleRecording);
    }

    private void Start()
    {
        obsWebSocketManager.RecordingState += HandleRecordingStateChange;
        activeToggleBtnColor = recToggleBtn.colors.normalColor;

        if (uiPanelSwitcher == null)
        {
            uiPanelSwitcher = FindObjectOfType<UiPanelSwitcher>();
        }

        if(recToggleBtn != null)
        {
            recToggleBtnIcon.iconUnicode = startIconUnicode;
            recToggleBtnText.text = startRecordingText;
            ColorBlock colors = recToggleBtn.colors;
            colors.normalColor = inactiveToggleBtnColor;
            recToggleBtn.colors = colors;
        }
    }

    private void Update()
    {
        Debug.Log("Current button text: " + recToggleBtnText.text);
    }

    private void HandleRecordingStateChange(bool isRecording)
    {
        if (isRecording)
        {
            recToggleBtnText.text = stopRecordingText;
            recToggleBtnIcon.iconUnicode = stopIconUnicode;
            ColorBlock colors = recToggleBtn.colors;
            colors.normalColor = activeToggleBtnColor;
            recToggleBtn.colors = colors;
        }

        else
        {
            recToggleBtnText.text = startRecordingText;
            recToggleBtnIcon.iconUnicode = startIconUnicode;
            ColorBlock colors = recToggleBtn.colors;
            colors.normalColor = inactiveToggleBtnColor;
            recToggleBtn.colors = colors;
        }
    }

    private void ToggleRecording()
    {
        if (obsWebSocketManager != null)
        {
            obsWebSocketManager.ToggleRecording();
        }
    }
}