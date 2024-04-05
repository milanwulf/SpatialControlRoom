using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.MaterialDesign.Icons;

public class UiRecordingPanel : MonoBehaviour
{
    /*
    private UiPanelSwitcher uiPanelSwitcher;
    [SerializeField] private OBSRecordingManager obsRecordingManager;

    //ToggleRecordingButton
    [SerializeField] private Button toggleRecordingBtn;

    [SerializeField] private MaterialIcon toggleRecordingBtnIcon;
    private string startIconUnicode = "e061";
    private string stopIconUnicode = "ef71";
    
    [SerializeField] private TextMeshProUGUI toggleRecordingBtnText;
    [SerializeField] private string stopRecordingText = "Stop Recording";
    [SerializeField] private string startRecordingText = "Start Recording";

    //CloseButton
    [SerializeField] private Button closeBtn;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(() => uiPanelSwitcher.HideAllUiPanels());
        toggleRecordingBtn.onClick.AddListener(ToggleRecording);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(() => uiPanelSwitcher.HideAllUiPanels());
        toggleRecordingBtn.onClick.RemoveListener(ToggleRecording);
    }

    private void Start()
    {

        if (uiPanelSwitcher == null)
        {
            uiPanelSwitcher = FindObjectOfType<UiPanelSwitcher>();
        }

        ChangeButtonAppearance();

    }

    private void ToggleRecording()
    {
        if (obsRecordingManager != null)
        {
            obsRecordingManager.ToggleRecording();
            ChangeButtonAppearance();
        }
    }

    private void ChangeButtonAppearance()
    {
        if (obsRecordingManager != null)
        {            
            if (obsRecordingManager.IsRecording)
            {
                toggleRecordingBtnText.text = stopRecordingText;
                toggleRecordingBtnIcon.iconUnicode = stopIconUnicode;
            }

            if (!obsRecordingManager.IsRecording)
            {
                toggleRecordingBtnText.text = startRecordingText;
                toggleRecordingBtnIcon.iconUnicode = startIconUnicode;
            }
        }
    }
    */
}