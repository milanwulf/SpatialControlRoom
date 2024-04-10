using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Flexalon;
using Flexalon.Templates;
using TMPro;
using Unity.VisualScripting;

public class UiInputItem : MonoBehaviour
{
    [SerializeField] private TemplateDynamicMaterial templateDynamicMaterial;
    [SerializeField] private Color defaultColor = new Color(0.85f, 0.85f, 0.85f);
    [SerializeField] private Color previewColor = new Color(0.19f, 084f, 0.29f);
    [SerializeField] private Color programColor = new Color(1f, 0.27f, 0.22f);

    public enum InputType
    {
        None,
        Scene,
        Preview,
        Program
    }

    [SerializeField] private InputType inputType;

    [SerializeField] private RawImage rawImage;
    
    private UiFeed.FeedType uiFeedType;
    public enum SelectedRenderTexture
    {
        NDI_Feed1,
        NDI_Feed2,
        NDI_Feed3
    }

    [SerializeField] private SelectedRenderTexture selectedRenderTexture;
    [SerializeField] private RenderTexture ndiFeed1RenderTexture;
    [SerializeField] private RenderTexture ndiFeed2RenderTexture;
    [SerializeField] private RenderTexture ndiFeed3RenderTexture;

    public enum RenderTextureOffset
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        None
    }

    [SerializeField] private RenderTextureOffset textureOffset;

    [SerializeField] private TextMeshProUGUI inputText;
    public enum DisplayText
    {
        None,
        Preview,
        Program,
        Scene1,
        Scene2,
        Scene3,
        Scene4,
        Scene5,
        Scene6,
        Scene7,
        Scene8,
        Scene9,
        Scene10
    }

    [SerializeField] private DisplayText displayText;
    private string inputTextString;
    private int inputSceneId;

    //Instantiate
    [SerializeField] BoxCollider protectedArea = null;
    private FlexalonInteractable flexalonInteractable;
    [SerializeField] UiFeedInstanceManger uiFeedInstanceManger = null;
    private Rect renderTextureOffset;
    private RenderTexture instanceRenderTexture;
    private Rect instanceRenderTextureOffset;

    private void Awake()
    {
        flexalonInteractable = GetComponent<FlexalonInteractable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (protectedArea == null)
        {
            protectedArea = GameObject.Find("ProtectedArea").GetComponent<BoxCollider>();
        }

        if(uiFeedInstanceManger == null)
        {
            uiFeedInstanceManger = GameObject.Find("UiFeedInstanceManger").GetComponent<UiFeedInstanceManger>();
        }

        SetInputType();
        SetRenderTexture();
        SetRenderTextureOffset();
        SetInputText();
    }

    private void OnEnable()
    {
        if (flexalonInteractable != null)
        {
            flexalonInteractable.DragEnd.AddListener(HandleDragEnd);
        }
    }

    private void OnDisable()
    {
        if (flexalonInteractable != null)
        {
            flexalonInteractable.DragEnd.RemoveListener(HandleDragEnd);
        }
    }

    private void HandleDragEnd(FlexalonInteractable interactable)
    {
        Vector3 currentPos = transform.position;
        if (!IsPosInsideProtectedArea(currentPos))
        {
            uiFeedInstanceManger.InstantiateNewFeed(currentPos, Quaternion.identity, instanceRenderTexture, instanceRenderTextureOffset, uiFeedType, inputTextString, inputSceneId);
        }
    }

    private bool IsPosInsideProtectedArea(Vector3 position)
    {
        if (protectedArea != null)
        {
            return protectedArea.bounds.Contains(position);
        }
        return false;
    }

    private void SetInputType()
    {
        if(templateDynamicMaterial != null)
        {
            switch (inputType)
            {
                case InputType.None:
                    templateDynamicMaterial.SetColor(defaultColor);
                    uiFeedType = UiFeed.FeedType.None;
                    break;
                case InputType.Scene:
                    templateDynamicMaterial.SetColor(defaultColor);
                    uiFeedType = UiFeed.FeedType.Scene;
                    break;
                case InputType.Preview:
                    templateDynamicMaterial.SetColor(previewColor);
                    uiFeedType = UiFeed.FeedType.Preview;
                    break;
                case InputType.Program:
                    templateDynamicMaterial.SetColor(programColor);
                    uiFeedType = UiFeed.FeedType.Program;
                    break;
            }
        }
    }

    private void SetRenderTexture()
    {
        if (rawImage != null)
        {
            // Use the RenderTexture from the SelectedRenderTexture enum as the texture for the RawImage component
            switch (selectedRenderTexture)
            {
                case SelectedRenderTexture.NDI_Feed1:
                    rawImage.texture = ndiFeed1RenderTexture;
                    instanceRenderTexture = ndiFeed1RenderTexture;
                    break;
                case SelectedRenderTexture.NDI_Feed2:
                    rawImage.texture = ndiFeed2RenderTexture;
                    instanceRenderTexture = ndiFeed2RenderTexture;
                    break;
                case SelectedRenderTexture.NDI_Feed3:
                    rawImage.texture = ndiFeed3RenderTexture;
                    instanceRenderTexture = ndiFeed3RenderTexture;
                    break;
            }
        }
    }

    private void SetRenderTextureOffset() 
    {         
        if (rawImage != null)
        {
            // Use the RenderTextureOffset enum to set the offset and tiling of the RawImage component's texture
            switch (textureOffset)
            {
                case RenderTextureOffset.TopLeft:
                    renderTextureOffset = new Rect(0, 0.5f, 0.5f, 0.5f);
                    rawImage.uvRect = renderTextureOffset;
                    instanceRenderTextureOffset = renderTextureOffset;
                    break;
                case RenderTextureOffset.TopRight:
                    renderTextureOffset = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    rawImage.uvRect = renderTextureOffset;
                    instanceRenderTextureOffset = renderTextureOffset;
                    break;
                case RenderTextureOffset.BottomLeft:
                    renderTextureOffset = new Rect(0, 0, 0.5f, 0.5f);
                    rawImage.uvRect = renderTextureOffset;
                    instanceRenderTextureOffset = renderTextureOffset;
                    break;
                case RenderTextureOffset.BottomRight:
                    renderTextureOffset = new Rect(0.5f, 0, 0.5f, 0.5f);
                    rawImage.uvRect = renderTextureOffset;
                    instanceRenderTextureOffset = renderTextureOffset;
                    break;
                case RenderTextureOffset.None:
                    renderTextureOffset = new Rect(0, 0, 1f, 1f);
                    rawImage.uvRect = renderTextureOffset;
                    instanceRenderTextureOffset = renderTextureOffset;
                    break;
            }
        }
    }

    private void SetInputText()
    {
        if (inputText != null)
        {
            switch (displayText)
            {
                case DisplayText.None:
                    inputText.text = "";
                    inputTextString = null;
                    inputSceneId = 0;
                    break;
                case DisplayText.Preview:
                    inputText.text = inputTextString = nameof(DisplayText.Preview);
                    inputSceneId = 0;
                    break;
                case DisplayText.Program:
                    inputText.text = inputTextString = nameof(DisplayText.Program);
                    inputSceneId = 0;
                    break;
                case DisplayText.Scene1:
                    inputText.text = inputTextString = nameof(DisplayText.Scene1);
                    inputSceneId = 1;
                    break;
                case DisplayText.Scene2:
                    inputText.text = inputTextString = nameof(DisplayText.Scene2);
                    inputSceneId = 2;
                    break;
                case DisplayText.Scene3:
                    inputText.text = inputTextString = nameof(DisplayText.Scene3);
                    inputSceneId = 3;
                    break;
                case DisplayText.Scene4:
                    inputText.text = inputTextString = nameof(DisplayText.Scene4);
                    inputSceneId = 4;
                    break;
                case DisplayText.Scene5:
                    inputText.text = inputTextString = nameof(DisplayText.Scene5);
                    inputSceneId = 5;
                    break;
                case DisplayText.Scene6:
                    inputText.text = inputTextString = nameof(DisplayText.Scene6);
                    inputSceneId = 6;
                    break;
                case DisplayText.Scene7:
                    inputText.text = inputTextString = nameof(DisplayText.Scene7);
                    inputSceneId = 7;
                    break;
                case DisplayText.Scene8:
                    inputText.text = inputTextString = nameof(DisplayText.Scene8);
                    inputSceneId = 8;
                    break;
                case DisplayText.Scene9:
                    inputText.text = inputTextString = nameof(DisplayText.Scene9);
                    inputSceneId = 9;
                    break;
                case DisplayText.Scene10:
                    inputText.text = inputTextString = nameof(DisplayText.Scene10);
                    inputSceneId = 10;
                    break;
            }
        }
    }
}
