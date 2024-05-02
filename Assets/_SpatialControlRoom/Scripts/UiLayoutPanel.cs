using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLayoutPanel : MonoBehaviour
{
    [SerializeField] private Button closeBtn;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(ClosePanel);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
