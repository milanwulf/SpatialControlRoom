using Google.MaterialDesign.Icons;
using System.Collections;
using System.Collections.Generic;
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
}
