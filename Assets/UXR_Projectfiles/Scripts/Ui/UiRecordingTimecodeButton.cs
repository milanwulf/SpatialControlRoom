using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiRecordingTimecodeButton : MonoBehaviour
{
    /*
    [SerializeField] private OBSRecordingManager obsRecordingManager;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private TextMeshProUGUI timecodeText;

    private Button button;
    private string defaultText;
    private Color defaultColor;
    private bool lastRecordingState;
    private float timeSinceLastUpdate = 0f;

    private void Start()
    {
        button = GetComponent<Button>();
        defaultText = timecodeText.text;
        defaultColor = button.colors.normalColor;
        lastRecordingState = !obsRecordingManager.IsRecording;
    }

    void Update()
    {
        if (obsRecordingManager != null)
        {
            if (obsRecordingManager.IsRecording != lastRecordingState)
            {
                lastRecordingState = obsRecordingManager.IsRecording;
                SwitchColor(obsRecordingManager.IsRecording);

                if (obsRecordingManager.IsRecording)
                {
                    timeSinceLastUpdate = 0f;
                }
            }

            if (obsRecordingManager.IsRecording)
            {
                timeSinceLastUpdate += Time.deltaTime;

                if (timeSinceLastUpdate >= 1f)
                {
                    timecodeText.text = obsRecordingManager.CurrentTimecode;
                    timeSinceLastUpdate = 0f;
                }
            }
            else if (timecodeText.text != defaultText)
            {
                timecodeText.text = defaultText;
            }
        }
    }

    private void SwitchColor(bool isActive)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = isActive ? defaultColor : inactiveColor;

        button.colors = colors;
    }
    */
}