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

    //TimecodeButton
    [Header("ActionBar Timecode Button")]
    [SerializeField] private GameObject recTimecodeBtnObject;
    [SerializeField] private Color recTimecodeBtnInactiveColor;
    private Color recTimecodeBtnRecordingColor;
    private Button recTimecodeBtnComponent;
    private TextMeshProUGUI recTimecodeBtnText;
    private MaterialIcon recTimecodeBtnIcon;

    //Variables
    private float recordingTimeInSeconds;

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

        if (uiPanelSwitcher == null)
        {
            uiPanelSwitcher = FindObjectOfType<UiPanelSwitcher>();
        }

        if(recTimecodeBtnObject != null)
        {
            recTimecodeBtnComponent = recTimecodeBtnObject.GetComponent<Button>();
            recTimecodeBtnText = recTimecodeBtnObject.GetComponentInChildren<TextMeshProUGUI>();
            recTimecodeBtnRecordingColor = recTimecodeBtnComponent.colors.normalColor;
            recTimecodeBtnIcon = recTimecodeBtnObject.GetComponentInChildren<MaterialIcon>();
            recTimecodeBtnIcon.iconUnicode = startIconUnicode;
        }

        if(recToggleBtn != null)
        {
            recToggleBtnIcon.iconUnicode = startIconUnicode;
            recToggleBtnText.text = startRecordingText;
        }
    }

    private void Update()
    {
        if(obsWebSocketManager != null && obsWebSocketManager.IsRecording)
        {
            recordingTimeInSeconds += Time.deltaTime;
            recTimecodeBtnIcon.iconUnicode = stopIconUnicode;
            UpdateRecordingTimecode();
        }
    }

    private void UpdateRecordingTimecode()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(recordingTimeInSeconds);
        string timecode = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        recTimecodeBtnText.text = timecode;
        recTimecodeBtnIcon.iconUnicode = stopIconUnicode;
        recToggleBtnText.text = stopRecordingText;
        recToggleBtnIcon.iconUnicode = stopIconUnicode;
    }

    public void ResetRecordingTimecode()
    {
        recordingTimeInSeconds = 0;
        recTimecodeBtnIcon.iconUnicode = startIconUnicode;
        recToggleBtnText.text = startRecordingText;
        recToggleBtnIcon.iconUnicode = startIconUnicode;
        UpdateRecordingTimecode();
    }

    private void ToggleRecording()
    {
        if (obsWebSocketManager != null)
        {
            obsWebSocketManager.ToggleRecording();
        }
    }
}