using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiStreamingTimecodeButton : MonoBehaviour
{
    [SerializeField] private OBSStreamingManager obsStreamingManager;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private TextMeshProUGUI timecodeText;

    private Button button;
    private string defaultText;
    private Color defaultColor;
    private bool lastStreamingState;
    private float timeSinceLastUpdate = 0f;

    private void Start()
    {
        button = GetComponent<Button>();
        defaultText = timecodeText.text;
        defaultColor = button.colors.normalColor;
        lastStreamingState = !obsStreamingManager.IsStreaming;
    }

    void Update()
    {
        if (obsStreamingManager != null)
        {
            if (obsStreamingManager.IsStreaming != lastStreamingState)
            {
                lastStreamingState = obsStreamingManager.IsStreaming;
                SwitchColor(obsStreamingManager.IsStreaming);

                if (obsStreamingManager.IsStreaming)
                {
                    timeSinceLastUpdate = 0f;
                }
            }

            if (obsStreamingManager.IsStreaming)
            {
                timeSinceLastUpdate += Time.deltaTime;

                if (timeSinceLastUpdate >= 1f)
                {
                    timecodeText.text = obsStreamingManager.CurrentTimecode;
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
}