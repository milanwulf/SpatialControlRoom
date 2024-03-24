using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPassthroughPanel : MonoBehaviour
{
    //CloseButton
    [SerializeField] private Button closeBtn;
    [SerializeField] private Toggle option1;
    [SerializeField] private Toggle option2;
    [SerializeField] private Toggle option3;
    [SerializeField] private Toggle option4;
    [SerializeField] private Slider opacitySlider;
    [SerializeField] private OVRPassthroughLayer passthroughLayer;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(ClosePanel);
        opacitySlider.onValueChanged.AddListener(OpacitySliderChanged);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(ClosePanel);
        opacitySlider.onValueChanged.RemoveListener(OpacitySliderChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(opacitySlider != null && passthroughLayer != null)
        {
            opacitySlider.value = passthroughLayer.textureOpacity;
        }
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void OpacitySliderChanged(float value)
    {
        if(passthroughLayer != null) { passthroughLayer.textureOpacity = value; }
    }
}
