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
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class UiFeed : MonoBehaviour
{
    public enum FeedType
    {
        None,
        Preview,
        Program,
        Scene
    }

    public FeedType feedType;
    public string localSceneName = null;
    public int localSceneId = 0;
    public SceneState localSceneState;

    private bool uiIsLocked = true;

    private FlexalonObject mainFlexalonObject;
    [SerializeField] private FlexalonObject feedFlexalonObject;

    private LazyFollow lazyFollower;

    [Header("Buttons")]
    [SerializeField] Button lockBtn;
    [SerializeField] MaterialIcon lockBtnIcon;
    private string unlockIconUnicode = "e898";
    private string lockIconUnicode = "e897";
    
    [SerializeField] Button renamingBtn;
    private FlexalonObject renamingBtnFlexalon;

    [SerializeField] UiToggleSwitch followSwitch;
    [SerializeField] UiToggleSwitch rotateSwitch;

    [SerializeField] Button landscapeBtn;
    [SerializeField] Button squareBtn;
    [SerializeField] Button portraitBtn;

    [SerializeField] Button duplicateBtn;
    [SerializeField] Button deleteBtnIcon;

    [SerializeField] Button videoFeedBtn;

    [Header("Sections")]
    [SerializeField] GameObject topBar;
    [SerializeField] GameObject transformOptions;
    [SerializeField] GameObject renamingBtnObj;
    [SerializeField] GameObject handles;

    [Header("Backgrounds")]
    [SerializeField] Image unlockStateBackground;
    float activeBackgroundAlpha;
    float disabledBackgroundAlpha = 0f;
    float backgroundAnimationDuration = 0.5f;
    [SerializeField] private Image videoPanelBackground;

    [Header("Managers")]
    private PositionFollowManager positionFollowManager;
    private UiFeedInstanceManger uiFeedInstanceManger;
    private OBSWebSocketManager obsWebSocketManager;

    [Header("Render Texture Input")]
    [SerializeField] private RawImage ndiFeedInput;

    [Header("Scene Name Text")]
    [SerializeField] private TextMeshProUGUI panelName;
    [SerializeField] private Image pannelNameBackground;

    [Header("Colors")]
    private Color defaultColor;
    [SerializeField] private Color previewColor = new Color32(50, 215, 75, 200);
    [SerializeField] private Color programColor = new Color32(255, 69, 58, 200);



    private void Awake()
    {
        positionFollowManager = FindObjectOfType<PositionFollowManager>();
        uiFeedInstanceManger = FindObjectOfType<UiFeedInstanceManger>();
        obsWebSocketManager = FindObjectOfType<OBSWebSocketManager>();
        defaultColor = videoPanelBackground.color;
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
        duplicateBtn.onClick.AddListener(DuplicateThisInstance);

        //Video Feed Button
        videoFeedBtn.onClick.AddListener(SetAsCurrentPreviewScene);

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
        duplicateBtn.onClick.RemoveListener(DuplicateThisInstance);

        //Video Feed Button
        videoFeedBtn.onClick.RemoveListener(SetAsCurrentPreviewScene);

    }

    // Start is called before the first frame update
    void Start()
    {
        lazyFollower = GetComponent<LazyFollow>();
        lazyFollower.positionFollowMode = LazyFollow.PositionFollowMode.None;
        //activate Rotation following by default
        lazyFollower.rotationFollowMode = LazyFollow.RotationFollowMode.LookAt;
        rotateSwitch.SetToggleState(true, false);

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
        if (followSwitch != null) followSwitch.SetToggleState(true);
    }

    public void DeactivatePositionFollowing()
    {
        lazyFollower.positionFollowMode = LazyFollow.PositionFollowMode.None;
        if (positionFollowManager.IsActiveFollower(this))
        {
            positionFollowManager.DeactivatePositionFollowing(this);
        }
        followSwitch.SetToggleState(false);
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
        if (rotateSwitch.isActiveAndEnabled)
        {
            rotateSwitch.SetToggleState(true);
        }
    }

    private void DeactivateRotationFollowing()
    {
        lazyFollower.rotationFollowMode = LazyFollow.RotationFollowMode.None;
        if (rotateSwitch.isActiveAndEnabled)
        {
            rotateSwitch.SetToggleState(false);
        }
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

    //UiFeedInstanceManger logic
    public void SetRenderTexture(RenderTexture renderTexture, Rect renderTextureOffset)
    {
        ndiFeedInput.texture = renderTexture;
        ndiFeedInput.uvRect = renderTextureOffset;
    }

    public void SetSceneIdAndType(int sceneId, string sceneName, FeedType setFeedType)
    {
        feedType = setFeedType;
        switch(feedType)
        {
            case FeedType.None:
                panelName.text = "Unnamed Source";
                pannelNameBackground.color = defaultColor;
                videoPanelBackground.color = defaultColor;
                break;
            case FeedType.Preview:
                panelName.text = "Preview";
                pannelNameBackground.color = previewColor;
                videoPanelBackground.color = previewColor;
                break;
            case FeedType.Program:
                panelName.text = "Program";
                pannelNameBackground.color = programColor;
                videoPanelBackground.color = programColor;
                break;
            case FeedType.Scene:
                if (sceneId != 0) //only set if not null
                {
                    localSceneId = sceneId;
                    if(sceneName != null)
                    {
                        localSceneName = sceneName;
                        panelName.text = localSceneName;
                    }
                }
                pannelNameBackground.color = defaultColor;
                videoPanelBackground.color = defaultColor;
                break;
        }
    }

    public enum SceneState
    {
        isNotActive,
        isCurrentPreview,
        isCurrentProgram
    }

    //public SceneState sceneState;

    public void SetSceneState(SceneState sceneState)
    {
        if(FeedType.Scene == feedType)
        {
            switch(sceneState)
            {
                case SceneState.isNotActive:
                    videoPanelBackground.color = defaultColor;
                    localSceneState = SceneState.isNotActive;
                    //Debug.Log(GetInstanceData().SceneName + "should be not active");
                    break;
                case SceneState.isCurrentPreview:
                    videoPanelBackground.color = previewColor;
                    localSceneState = SceneState.isCurrentPreview;

                    //Debug.Log(GetInstanceData().SceneName + "isCurrentPreview");
                    break;
                case SceneState.isCurrentProgram:
                    videoPanelBackground.color = programColor; ;
                    localSceneState = SceneState.isCurrentProgram;
                    //Debug.Log(GetInstanceData().SceneName + "should be current program");
                    break;
            }
        }
    }
    
    private void RemoveThisInstance()
    {
        uiFeedInstanceManger.RemoveFeedInstance(this);
    }

    private void DuplicateThisInstance()
    {
        uiFeedInstanceManger.DuplicateUiFeedInstance(this);
    }

    private void SetAsCurrentPreviewScene()
    {
        if(feedType == FeedType.Scene && localSceneState != SceneState.isCurrentProgram && localSceneState != SceneState.isCurrentPreview) //only set if not already set as Program or Preview
        {
            obsWebSocketManager.SetPreviewSceneByIndex(localSceneId);
        }
    }

    public class InstanceData
    {
        public RenderTexture RenderTexture { get; set; }
        public Rect Offset { get; set; }
        public FeedType FeedType { get; set; }
        public string SceneName { get; set; }
        public int SceneId { get; set; }
        public SceneState SceneState { get; set;}
    }

    public InstanceData GetInstanceData()
    {
        return new InstanceData
        {
            RenderTexture = ndiFeedInput.texture as RenderTexture,
            Offset = ndiFeedInput.uvRect,
            FeedType = feedType,
            SceneName = localSceneName,
            SceneId = localSceneId,
            SceneState = localSceneState
        };
    }
}
