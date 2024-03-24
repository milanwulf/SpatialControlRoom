using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPassthroughPanel : MonoBehaviour
{
    //CloseButton
    [SerializeField] private Button closeBtn;
    [SerializeField] private Toggle toggle1;
    [SerializeField] private Toggle toggle2;
    [SerializeField] private Toggle toggle3;
    [SerializeField] private Toggle toggle4;
    [SerializeField] private Slider opacitySlider;
    [SerializeField] private OVRPassthroughLayer passthroughLayer;

    [Space(20)]
    [Header("Passthrough Style 1 is default")]
    [Header("Passthrough Style 2")] 
    [SerializeField] private bool style2EdgeRendering;
    [SerializeField] private Color style2EdgeColor;
    [SerializeField] private Gradient style2Gradient;
    [Header("Passthrough Style 2")]
    [SerializeField] private bool style3EdgeRendering;
    [SerializeField] private Color style3EdgeColor;
    [SerializeField] private Gradient style3Gradient;
    [Header("Passthrough Style 3")]
    [SerializeField] private bool style4EdgeRendering;
    [SerializeField] private Color style4EdgeColor;
    [SerializeField] private Gradient style4Gradient;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(ClosePanel);
        opacitySlider.onValueChanged.AddListener(OpacitySliderChanged);

        toggle1.onValueChanged.AddListener(isOn => { if (isOn) ChangePassthroughStyle(); });
        toggle2.onValueChanged.AddListener(isOn => { if (isOn) ChangePassthroughStyle(); });
        toggle3.onValueChanged.AddListener(isOn => { if (isOn) ChangePassthroughStyle(); });
        toggle4.onValueChanged.AddListener(isOn => { if (isOn) ChangePassthroughStyle(); });
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(ClosePanel);
        opacitySlider.onValueChanged.RemoveListener(OpacitySliderChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set current opacity value from inspector
        if(opacitySlider != null && passthroughLayer != null)
        {
            opacitySlider.value = passthroughLayer.textureOpacity;
        }

        //set default style
        toggle1.isOn = true;

    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void OpacitySliderChanged(float value)
    {
        if(passthroughLayer != null) { passthroughLayer.textureOpacity = value; }
    }

    private void ChangePassthroughStyle()
    {
        Debug.Log("is called!");
        if(toggle1.isOn)
        {
            passthroughLayer.edgeRenderingEnabled = false;
            
            passthroughLayer.DisableColorMap();
        }

        if (toggle2.isOn)
        {
            passthroughLayer.edgeRenderingEnabled = style2EdgeRendering;
            passthroughLayer.edgeColor = style2EdgeColor;
            passthroughLayer.SetColorMapControls(0f, 0f, 0f, style2Gradient, OVRPassthroughLayer.ColorMapEditorType.GrayscaleToColor);
        }

        if (toggle3.isOn)
        {
            passthroughLayer.edgeRenderingEnabled = style3EdgeRendering;
            passthroughLayer.edgeColor = style3EdgeColor;
            passthroughLayer.SetColorMapControls(0.5f, 0f, 0f, style3Gradient, OVRPassthroughLayer.ColorMapEditorType.GrayscaleToColor);
        }

        if (toggle4.isOn)
        {
            passthroughLayer.edgeRenderingEnabled = style4EdgeRendering;
            passthroughLayer.edgeColor = style4EdgeColor;
            passthroughLayer.SetColorMapControls(0.5f, 0f, 0f, style4Gradient, OVRPassthroughLayer.ColorMapEditorType.GrayscaleToColor);
        }
    }

}
