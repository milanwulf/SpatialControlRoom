using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.MaterialDesign.Icons;

public class UiStreamingPanel : MonoBehaviour
{
    private UiPanelSwitcher uiPanelSwitcher;
    [SerializeField] private OBSWebSocketManager obsWebSocketManager;

    //ToggleRecordingButton
    [Header("Toggle Stream Button")]
    [SerializeField] private Button streamToggleBtn;
    [SerializeField] private Color inactiveToggleBtnColor;
    private Color activeToggleBtnColor;

    [SerializeField] private MaterialIcon streamToggleBtnIcon;
    private string startIconUnicode = "e0e2";
    private string stopIconUnicode = "e0e3";

    [SerializeField] private TextMeshProUGUI streamToggleBtnText;
    [SerializeField] private string stopStreamText = "Stop Streaming";
    [SerializeField] private string startStreamText = "Start Streaming";

    //CloseButton
    [Header("Close Panel Button")]
    [SerializeField] private Button closeBtn;


    private void OnEnable()
    {
        closeBtn.onClick.AddListener(() => uiPanelSwitcher.HideAllUiPanels());
        streamToggleBtn.onClick.AddListener(ToggleStream);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(() => uiPanelSwitcher.HideAllUiPanels());
        streamToggleBtn.onClick.RemoveListener(ToggleStream);
    }

    private void Start()
    {
        activeToggleBtnColor = streamToggleBtn.colors.normalColor;

        if (uiPanelSwitcher == null)
        {
            uiPanelSwitcher = FindObjectOfType<UiPanelSwitcher>();
        }

        if (streamToggleBtn != null)
        {
            streamToggleBtnIcon.iconUnicode = startIconUnicode;
            streamToggleBtnText.text = startStreamText;
            ColorBlock colors = streamToggleBtn.colors;
            colors.normalColor = inactiveToggleBtnColor;
            streamToggleBtn.colors = colors;
            obsWebSocketManager.StreamingState += HandleStreamStateChange;
        }
    }

    private void Update()
    {
        //Debug.Log("Current button text: " + streamToggleBtnText.text);
    }

    private void HandleStreamStateChange(bool isStreaming)
    {
        if (isStreaming)
        {
            streamToggleBtnText.text = stopStreamText;
            streamToggleBtnIcon.iconUnicode = stopIconUnicode;
            ColorBlock colors = streamToggleBtn.colors;
            colors.normalColor = activeToggleBtnColor;
            streamToggleBtn.colors = colors;
        }

        else
        {
            streamToggleBtnText.text = startStreamText;
            streamToggleBtnIcon.iconUnicode = startIconUnicode;
            ColorBlock colors = streamToggleBtn.colors;
            colors.normalColor = inactiveToggleBtnColor;
            streamToggleBtn.colors = colors;
        }
    }

    private void ToggleStream()
    {
        if (obsWebSocketManager != null)
        {
            obsWebSocketManager.ToggleStreaming();
        }
    }
}