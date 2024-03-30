using Google.MaterialDesign.Icons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Flexalon;
using Meta.WitAi;

public class UiFeed : MonoBehaviour
{
    private bool uiIsLocked = true;

    private FlexalonObject mainFlexalonObject;

    //LockButton
    [SerializeField] Button lockBtn;
    [SerializeField] MaterialIcon lockBtnIcon;
    private string unlockIconUnicode = "e898";
    private string lockIconUnicode = "e897";

    //Buttons
    [SerializeField] Button renamingBtn;
    private FlexalonObject renamingBtnFlexalon;

    [SerializeField] Button followBtn;
    [SerializeField] Button rotateBtn;

    [SerializeField] Button landscapeBtn;
    [SerializeField] Button squareBtn;
    [SerializeField] Button portraitBtn;

    [SerializeField] Button duplicateBtn;
    [SerializeField] Button deleteBtnIcon;

    //Sections
    [SerializeField] GameObject topBar;
    [SerializeField] GameObject transformOptions;
    [SerializeField] GameObject renamingBtnObj;
    [SerializeField] GameObject handles;

    [SerializeField] Image unlockStateBackground;
    float activeBackgroundAlpha;
    float disabledBackgroundAlpha = 0f;
    float backgroundAnimationDuration = 0.5f;

    private void Awake()
    {
        mainFlexalonObject = GetComponent<FlexalonObject>();
        renamingBtnFlexalon = renamingBtn.GetComponent<FlexalonObject>();
        activeBackgroundAlpha = unlockStateBackground.color.a;
        InitialLockState();
    }

    private void OnEnable()
    {
        lockBtn.onClick.AddListener(ToggleLockState);
    }

    private void OnDisable()
    {
        lockBtn.onClick.RemoveListener(ToggleLockState);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void InitialLockState()
    {
        SetUIElementsActive(!uiIsLocked);
        mainFlexalonObject.ForceUpdate();
        float initialAlpha = uiIsLocked ? disabledBackgroundAlpha : activeBackgroundAlpha;
        unlockStateBackground.color = new Color(unlockStateBackground.color.r, unlockStateBackground.color.g, unlockStateBackground.color.b, initialAlpha);
    }

    private void ToggleLockState()
    {
        uiIsLocked = !uiIsLocked; // Toggle the state

        SetUIElementsActive(!uiIsLocked);

        mainFlexalonObject.ForceUpdate();
        float startAlpha = uiIsLocked ? activeBackgroundAlpha : disabledBackgroundAlpha;
        float endAlpha = uiIsLocked ? disabledBackgroundAlpha : activeBackgroundAlpha;
        StartCoroutine(AnimateBackground(startAlpha, endAlpha, backgroundAnimationDuration));
    }

    private void SetUIElementsActive(bool isActive)
    {
        topBar.SetActive(isActive);
        transformOptions.SetActive(isActive);
        renamingBtnObj.SetActive(isActive);
        handles.SetActive(isActive);
        lockBtnIcon.iconUnicode = isActive ? unlockIconUnicode : lockIconUnicode;
        renamingBtnFlexalon.SkipLayout = !isActive;
    }

    private IEnumerator AnimateBackground(float startAlpha, float endAlpha, float duration)
    {
        float currentTime = 0.0f;
        Color currentColor = unlockStateBackground.color;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            unlockStateBackground.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        unlockStateBackground.color = new Color(currentColor.r, currentColor.g, currentColor.b, endAlpha);
    }
}
