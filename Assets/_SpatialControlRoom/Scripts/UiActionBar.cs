using Google.MaterialDesign.Icons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiActionBar : MonoBehaviour
{
    [SerializeField] UiPanelSwitcher uiPanelSwitcher;
    [SerializeField] OBSWebSocketManager obsWebSocketManager;

    //UiActionBar Animation
    Animator actionBarAnimator;
    string triggerCloseAnimation = "CloseActionBar";
    string triggerOpenAnimation = "OpenActionBar";
    string closeIdleState = "UiActionBarCloseIdle";
    string openIdleState = "UiActionBarOpenIdle";

    [Header("Action Bar Button")]
    [SerializeField] Button actionBarBtn;
    [SerializeField] MaterialIcon actionBarBtnIcon;
    [SerializeField] string openIconUnicode;
    [SerializeField] string closeIconUnicode;

    [Header("Recording Button")]
    [SerializeField] Button recordingBtn;
    [SerializeField] Color inactiveRecBtnColor;
    [SerializeField] TextMeshProUGUI recBtnText;
    [SerializeField] MaterialIcon recBtnIcon;
    string startRecIcon = "e061";
    string stopRecIcon = "ef71";
    private string defaultRecBtnText;
    private Color recordingRecBtnColor;
    private bool recBtnTimecodeIsRunning;

    [Header("Streaming Button")]
    [SerializeField] Button streamingBtn;
    [SerializeField] Color inactiveStreamBtnColor;
    [SerializeField] TextMeshProUGUI streamBtnText;
    [SerializeField] MaterialIcon streamBtnIcon;
    string startStreamIcon = "e0e2";
    string stopStreamIcon = "e0e3";
    private string defaultStreamBtnText;
    private Color streamingStreamBtnColor;
    private bool streamBtnTimecodeIsRunning;

    [Header("Menu Buttons")]
    [SerializeField] Button layoutBtn;
    [SerializeField] Button inputsBtn;
    [SerializeField] Button passthroughBtn;
    [SerializeField] Button settingsBtn;

    [Header("Clock")]
    private bool is24HourFormat = false;
    [SerializeField] TextMeshProUGUI clockText;

    [Header("Battery Status")]
    [SerializeField] MaterialIcon batteryIcon;
    string batteryChargingIconUnicode = "e1a3";
    string batteryNotChargingIconUnicode = "e19c";
    string batteryUnknownIconUnicode = "e1a6";
    string battery0 = "ebdc";
    string battery12 = "ebd9";
    string battery25 = "ebe0";
    string battery37 = "ebdd";
    string battery50 = "ebe2";
    string battery62 = "ebd4";
    string battery75 = "ebd2";
    string batteryFull = "e1a4";

    private void OnEnable()
    {
        actionBarBtn.onClick.AddListener(ToggleActionBar);

        //UiPanelSwitcher
        recordingBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(0); });
        streamingBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(1); });
        layoutBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(2); });
        inputsBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(3); });
        passthroughBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(4); }); //index might change!
        settingsBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(5); }); //shows connection settings for testing, change index later!
    }

    private void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        actionBarAnimator = GetComponent<Animator>();
        StartCoroutine(UpdateClock());
        StartCoroutine(UpdateBatteryStatus());

        //Recording Button
        if (recordingBtn != null)
        {
            defaultRecBtnText = recBtnText.text;
            recordingRecBtnColor = recordingBtn.colors.normalColor;
            UpdateRecordingButtonState(false);
            obsWebSocketManager.RecordingState += UpdateRecordingButtonState;
        }

        //Streaming Button
        if (streamingBtn != null)
        {
            defaultStreamBtnText = streamBtnText.text;
            streamingStreamBtnColor = streamingBtn.colors.normalColor;
            UpdateStreamingButtonState(false);
            obsWebSocketManager.StreamingState += UpdateStreamingButtonState;
        }
    }

    void ToggleActionBar()
    {
        if (actionBarAnimator != null)
        {
            if (actionBarAnimator.GetCurrentAnimatorStateInfo(0).IsName(openIdleState))
            {
                actionBarAnimator.SetTrigger(triggerCloseAnimation);
                if (openIconUnicode != null)
                {
                    uiPanelSwitcher.HideAllUiPanels();
                    actionBarBtnIcon.iconUnicode = openIconUnicode;
                }
            }

            else if (actionBarAnimator.GetCurrentAnimatorStateInfo(0).IsName(closeIdleState))
            {
                actionBarAnimator.SetTrigger(triggerOpenAnimation);
                if (closeIconUnicode != null)
                {
                    actionBarBtnIcon.iconUnicode = closeIconUnicode;
                }
            }
        }

        else
        {
            Debug.Log("UiActionBar has no Animator!");
        }
    }

    private IEnumerator UpdateClock()
    {
        while (true)
        {   
            string format = is24HourFormat ? "HH:mm:ss" : "hh:mm:ss tt";
            clockText.text = DateTime.Now.ToString(format, new CultureInfo("en-US"));
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator UpdateBatteryStatus()
    {
        while (true)
        {
            float batteryLevel = SystemInfo.batteryLevel;
            BatteryStatus batteryStatus = SystemInfo.batteryStatus;
            
            if(batteryLevel < 0)
            {
                batteryIcon.iconUnicode = batteryUnknownIconUnicode;
            }

            else if(batteryStatus == BatteryStatus.Charging)
            {
                batteryIcon.iconUnicode = batteryChargingIconUnicode;
            }

            else if(batteryStatus == BatteryStatus.NotCharging)
            {
                batteryIcon.iconUnicode = batteryNotChargingIconUnicode;
            }

            else
            {
                switch(batteryLevel * 100)
                {
                    case float level when level <= 12:
                        batteryIcon.iconUnicode = battery0;
                        break;
                    case float level when level <= 25:
                        batteryIcon.iconUnicode = battery12;
                        break;
                    case float level when level <= 37:
                        batteryIcon.iconUnicode = battery25;
                        break;
                    case float level when level <= 50:
                        batteryIcon.iconUnicode = battery37;
                        break;
                    case float level when level <= 62:
                        batteryIcon.iconUnicode = battery50;
                        break;
                    case float level when level <= 75:
                        batteryIcon.iconUnicode = battery62;
                        break;
                    case float level when level <= 100:
                        batteryIcon.iconUnicode = battery75;
                        break;
                    default:
                        batteryIcon.iconUnicode = batteryFull;
                        break;
                }
            }

            yield return new WaitForSeconds(5);
        }
    }

    //Recording Button Timecode Logic
    private void UpdateRecordingButtonState(bool isRecording)
    {
        Debug.Log("UpdateRecordingButtonState is called: " + isRecording);

        if (isRecording)
        {
            recBtnTimecodeIsRunning = true;
            StartCoroutine(RecordingTimecode());
            recBtnIcon.iconUnicode = stopRecIcon;
            ColorBlock colors = recordingBtn.colors;
            colors.normalColor = recordingRecBtnColor;
            recordingBtn.colors = colors;
        }

        else
        {
            recBtnTimecodeIsRunning = false;
            recBtnText.text = defaultRecBtnText;
            recBtnIcon.iconUnicode = startRecIcon;
            ColorBlock colors = recordingBtn.colors;
            colors.normalColor = inactiveRecBtnColor;
            recordingBtn.colors = colors;
        }
    }

    private IEnumerator RecordingTimecode() //TODO: sync with OBS recording timecode
    {
        int hours = 0, minutes = 0, seconds = 0;
        while (recBtnTimecodeIsRunning)
        {
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes >= 60)
                {
                    minutes = 0;
                    hours++;
                }
            }

            recBtnText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
            yield return new WaitForSeconds(1);
        }
    }

    //Streaming Button Timecode Logic
    private void UpdateStreamingButtonState(bool isStreaming)
    {
        Debug.Log("UpdateStreamingButtonState is called: " + isStreaming);
        if (isStreaming)
        {
            streamBtnTimecodeIsRunning = true;
            StartCoroutine(StreamingTimecode());
            streamBtnIcon.iconUnicode = stopStreamIcon;
            ColorBlock colors = streamingBtn.colors;
            colors.normalColor = streamingStreamBtnColor;
            streamingBtn.colors = colors;
        }

        else
        {
            streamBtnTimecodeIsRunning = false;
            streamBtnText.text = defaultStreamBtnText;
            streamBtnIcon.iconUnicode = startStreamIcon;
            ColorBlock colors = streamingBtn.colors;
            colors.normalColor = inactiveStreamBtnColor;
            streamingBtn.colors = colors;
        }
    }

    private IEnumerator StreamingTimecode() //TODO: sync with OBS streaming timecode
    {
        int hours = 0, minutes = 0, seconds = 0;
        while (streamBtnTimecodeIsRunning)
        {
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes >= 60)
                {
                    minutes = 0;
                    hours++;
                }
            }

            streamBtnText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
            yield return new WaitForSeconds(1);
        }
    }
}
