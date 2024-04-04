using Google.MaterialDesign.Icons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Flexalon;
using Meta.WitAi;
using UnityEngine.XR.Interaction.Toolkit.UI;
using Christina.UI;

public class UiFeed : MonoBehaviour
{
    private bool uiIsLocked = true;

    private FlexalonObject mainFlexalonObject;
    [SerializeField] private FlexalonObject feedFlexalonObject;

    private LazyFollow lazyFollower;

    //LockButton
    [SerializeField] Button lockBtn;
    [SerializeField] MaterialIcon lockBtnIcon;
    private string unlockIconUnicode = "e898";
    private string lockIconUnicode = "e897";

    //Buttons
    [SerializeField] Button renamingBtn;
    private FlexalonObject renamingBtnFlexalon;

    [SerializeField] UiToggleSwitch followSwitch;
    [SerializeField] UiToggleSwitch rotateSwitch;

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

    private PositionFollowManager positionFollowManager;
    private UiFeedInstanceManger uiFeedInstanceManger;

    //RenderTexture Input
    [Header("Render Texture Input")]
    [SerializeField] private RawImage ndiFeedInput;
    

    private void Awake()
    {
        positionFollowManager = FindObjectOfType<PositionFollowManager>();
        uiFeedInstanceManger = FindObjectOfType<UiFeedInstanceManger>();
    }

    private void OnEnable()
    {
        lockBtn.onClick.AddListener(ToggleLockState);

        //Transform
        followSwitch.OnToggleOn.AddListener(() => HandlePositionFollowing(true));
        followSwitch.OnToggleOff.AddListener(() => HandlePositionFollowing(false));
        rotateSwitch.OnToggleOn.AddListener(ActivateRotationFollowing);
        rotateSwitch.OnToggleOff.AddListener(DeactivateRotationFollowing);
        positionFollowManager.RegisterPosFollower(this);

        //Aspect Ratio
        landscapeBtn.onClick.AddListener(() => SetAspectRatio("landscape"));
        squareBtn.onClick.AddListener(() => SetAspectRatio("square"));
        portraitBtn.onClick.AddListener(() => SetAspectRatio("portrait"));

        //Delete Button
        deleteBtnIcon.onClick.AddListener(RemoveThisInstance);

        //Duplicate Button
        duplicateBtn.onClick.AddListener(DublicateThisInstance);

    }

    private void OnDisable()
    {
        lockBtn.onClick.RemoveListener(ToggleLockState);

        //Transform
        followSwitch.OnToggleOn.RemoveListener(() => HandlePositionFollowing(true));
        followSwitch.OnToggleOff.RemoveListener(() => HandlePositionFollowing(false));
        rotateSwitch.OnToggleOn.RemoveListener(ActivateRotationFollowing);
        rotateSwitch.OnToggleOff.RemoveListener(DeactivateRotationFollowing);
        positionFollowManager.DeregisterPosFollower(this);

        //Aspect Ratio
        landscapeBtn.onClick.RemoveListener(() => SetAspectRatio("landscape"));
        squareBtn.onClick.RemoveListener(() => SetAspectRatio("square"));
        portraitBtn.onClick.RemoveListener(() => SetAspectRatio("portrait"));

        //Delete Button
        deleteBtnIcon.onClick.RemoveListener(RemoveThisInstance);

        //Duplicate Button
        duplicateBtn.onClick.RemoveListener(DublicateThisInstance);

    }

    // Start is called before the first frame update
    void Start()
    {
        lazyFollower = GetComponent<LazyFollow>();
        lazyFollower.positionFollowMode = LazyFollow.PositionFollowMode.None;
        lazyFollower.rotationFollowMode = LazyFollow.RotationFollowMode.None;
        mainFlexalonObject = GetComponent<FlexalonObject>();
        renamingBtnFlexalon = renamingBtn.GetComponent<FlexalonObject>();
        activeBackgroundAlpha = unlockStateBackground.color.a;
        InitialLockState();

        uiFeedInstanceManger.RegisterUiFeedInstance(this); //Register this instance in the manager if already in scene
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

    //Transform Handling
    public void ActivatePositionFollowing()
    {
        lazyFollower.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
        if (followSwitch != null) followSwitch.SetToggleStateDirectly(true);
    }

    public void DeactivatePositionFollowing()
    {
        lazyFollower.positionFollowMode = LazyFollow.PositionFollowMode.None;
        if (positionFollowManager.IsActiveFollower(this))
        {
            positionFollowManager.DeactivatePositionFollowing(this);
        }
        followSwitch.SetToggleStateDirectly(false);
    }

    private void HandlePositionFollowing(bool isOn)
    {
        if (isOn)
        {
            positionFollowManager.RequestFollowActivation(this);
        }
        else
        {
            if (positionFollowManager.IsActiveFollower(this))
            {
                DeactivatePositionFollowing();
            }
        }
    }

    private void ActivateRotationFollowing()
    {
        lazyFollower.rotationFollowMode = LazyFollow.RotationFollowMode.LookAt;
    }

    private void DeactivateRotationFollowing()
    {
        lazyFollower.rotationFollowMode = LazyFollow.RotationFollowMode.None;
    }

    //Aspect Ratio Handling
    private void SetAspectRatio(string aspectRatio)
    {
        switch (aspectRatio)
        {
            case "landscape":
                feedFlexalonObject.Size = new Vector2(1920, 1080);

                break;
            case "square":
                feedFlexalonObject.Size = new Vector2(1920, 1920);
                break;
            case "portrait":
                feedFlexalonObject.Size = new Vector2(1080, 1920);
                break;
        }

        //feedFlexalonObject.ForceUpdate();
        mainFlexalonObject.ForceUpdate();
    }

    public void SetRenderTexture(RenderTexture renderTexture, Rect renderTextureOffset)
    {
        ndiFeedInput.texture = renderTexture;
        ndiFeedInput.uvRect = renderTextureOffset;
    }

    //UiFeedInstanceManger logic
    private void RemoveThisInstance()
    {
        uiFeedInstanceManger.RemoveFeedInstance(this);
    }

    private void DublicateThisInstance()
    {
        uiFeedInstanceManger.DublicateUiFeedInstance(this);
    }

    public class InstanceData
    {
        public RenderTexture RenderTexture { get; set; }
        public Rect Offset { get; set; }
    }

    public InstanceData GetInstanceData()
    {
        return new InstanceData
        {
            RenderTexture = ndiFeedInput.texture as RenderTexture,
            Offset = ndiFeedInput.uvRect
        };
    }
}
