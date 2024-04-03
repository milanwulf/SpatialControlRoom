using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Flexalon;
using Flexalon.Templates;
using TMPro;

public class UiInputItem : MonoBehaviour
{
    [SerializeField] private TemplateDynamicMaterial templateDynamicMaterial;
    [SerializeField] private Color defaultColor = new Color(0.85f, 0.85f, 0.85f);
    [SerializeField] private Color previewColor = new Color(0.19f, 084f, 0.29f);
    [SerializeField] private Color programColor = new Color(1f, 0.27f, 0.22f);

    public enum InputType
    {
        Default,
        Preview,
        Program
    }

    [SerializeField] private InputType inputType;

    [SerializeField] private RawImage rawImage;

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

    // Start is called before the first frame update
    void Start()
    {
        SetInputType();
        SetRenderTexture();
        SetRenderTextureOffset();
        SetInputText();
    }

    private void SetInputType()
    {
        if(templateDynamicMaterial != null)
        {
            switch (inputType)
            {
                case InputType.Default:
                    templateDynamicMaterial.SetColor(defaultColor);
                    break;
                case InputType.Preview:
                    templateDynamicMaterial.SetColor(previewColor);
                    break;
                case InputType.Program:
                    templateDynamicMaterial.SetColor(programColor);
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
                    break;
                case SelectedRenderTexture.NDI_Feed2:
                    rawImage.texture = ndiFeed2RenderTexture;
                    break;
                case SelectedRenderTexture.NDI_Feed3:
                    rawImage.texture = ndiFeed3RenderTexture;
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
                    rawImage.uvRect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    break;
                case RenderTextureOffset.TopRight:
                        rawImage.uvRect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case RenderTextureOffset.BottomLeft:
                        rawImage.uvRect = new Rect(0, 0, 0.5f, 0.5f);
                    break;
                case RenderTextureOffset.BottomRight:
                        rawImage.uvRect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
                case RenderTextureOffset.None:
                        rawImage.uvRect = new Rect(0, 0, 1f, 1f);
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
                    break;
                case DisplayText.Preview:
                    inputText.text = "Preview";
                    break;
                case DisplayText.Program:
                    inputText.text = "Program";
                    break;
                case DisplayText.Scene1:
                    inputText.text = "Scene 1";
                    break;
                case DisplayText.Scene2:
                    inputText.text = "Scene 2";
                    break;
                case DisplayText.Scene3:
                    inputText.text = "Scene 3";
                    break;
                case DisplayText.Scene4:
                    inputText.text = "Scene 4";
                    break;
                case DisplayText.Scene5:
                    inputText.text = "Scene 5";
                    break;
                case DisplayText.Scene6:
                    inputText.text = "Scene 6";
                    break;
                case DisplayText.Scene7:
                    inputText.text = "Scene 7";
                    break;
                case DisplayText.Scene8:
                    inputText.text = "Scene 8";
                    break;
                case DisplayText.Scene9:
                    inputText.text = "Scene 9";
                    break;
                case DisplayText.Scene10:
                    inputText.text = "Scene 10";
                    break;
            }
        }
    }
}
