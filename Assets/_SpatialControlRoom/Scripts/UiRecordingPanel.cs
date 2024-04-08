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

        if (uiPanelSwitcher == null)
        {
            uiPanelSwitcher = FindObjectOfType<UiPanelSwitcher>();
        }

        if(recToggleBtn != null)
        {
            recToggleBtnIcon.iconUnicode = startIconUnicode;
            recToggleBtnText.text = startRecordingText;
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
            Debug.Log("Nun sollte Stop Recording angezeigt werden");
            recToggleBtnText.text = stopRecordingText;
            recToggleBtnIcon.iconUnicode = stopIconUnicode;
        }
        else
        {
            Debug.Log("Nun sollte Start Recording angezeigt werden");
            recToggleBtnText.text = startRecordingText;
            recToggleBtnIcon.iconUnicode = startIconUnicode;
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