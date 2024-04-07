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

    //UiActionBar Animation
    Animator actionBarAnimator;
    string triggerCloseAnimation = "CloseActionBar";
    string triggerOpenAnimation = "OpenActionBar";
    string closeIdleState = "UiActionBarCloseIdle";
    string openIdleState = "UiActionBarOpenIdle";

    //ActionBarButton
    [SerializeField] Button actionBarBtn;
    [SerializeField] MaterialIcon actionBarBtnIcon;
    [SerializeField] string openIconUnicode;
    [SerializeField] string closeIconUnicode;

    //Buttons
    [SerializeField] Button recordingBtn;
    [SerializeField] Button streamingBtn;
    [SerializeField] Button layoutBtn;
    [SerializeField] Button inputsBtn;
    [SerializeField] Button passthroughBtn;
    [SerializeField] Button settingsBtn;

    //Clock
    private bool is24HourFormat = false;
    [SerializeField] TextMeshProUGUI clockText;

    //Battery
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
        //layoutBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(X); }); //TODO: implement UiPanel
        inputsBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(2); });
        passthroughBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(3); }); //index might change!
        settingsBtn.onClick.AddListener(() => { uiPanelSwitcher.ShowUiPanel(4); }); //shows connection settings for testing, change index later!
    }

    // Start is called before the first frame update
    void Start()
    {
        actionBarAnimator = GetComponent<Animator>();
        StartCoroutine(UpdateClock());
        StartCoroutine(UpdateBatteryStatus());
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
}
