using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.MaterialDesign.Icons;

public class UiStreamingPanel : MonoBehaviour
{
    private UiPanelSwitcher uiPanelSwitcher;
    [SerializeField] private OBSStreamingManager obsStreamingManager;

    //ToggleStreamingButton
    [SerializeField] private Button toggleStreamingBtn;

    [SerializeField] private MaterialIcon toggleStreamingBtnIcon;
    private string startIconUnicode = "e061";
    private string stopIconUnicode = "ef71";
    
    [SerializeField] private TextMeshProUGUI toggleStreamingBtnText;
    [SerializeField] private string stopStreamingText = "Stop Streaming";
    [SerializeField] private string startStreamingText = "Start Streaming";

    //CloseButton
    [SerializeField] private Button closeBtn;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(() => uiPanelSwitcher.HideAllUiPanels());
        toggleStreamingBtn.onClick.AddListener(ToggleStreaming);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(() => uiPanelSwitcher.HideAllUiPanels());
        toggleStreamingBtn.onClick.RemoveListener(ToggleStreaming);
    }

    private void Start()
    {

        if (uiPanelSwitcher == null)
        {
            uiPanelSwitcher = FindObjectOfType<UiPanelSwitcher>();
        }

        ChangeButtonAppearance();

    }

    private void ToggleStreaming()
    {
        if (obsStreamingManager != null)
        {
            obsStreamingManager.ToggleStreaming();
            ChangeButtonAppearance();
        }
    }

    private void ChangeButtonAppearance()
    {
        if (obsStreamingManager != null)
        {            
            if (obsStreamingManager.IsStreaming)
            {
                toggleStreamingBtnText.text = stopStreamingText;
                toggleStreamingBtnIcon.iconUnicode = stopIconUnicode;
            }

            if (!obsStreamingManager.IsStreaming)
            {
                toggleStreamingBtnText.text = startStreamingText;
                toggleStreamingBtnIcon.iconUnicode = startIconUnicode;
            }
        }
    }
}