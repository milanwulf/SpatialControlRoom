using UnityEngine;

public class UiPanelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] uiPanels;

    private void Start()
    {
        HideAllUiPanels();
    }

    public void ShowUiPanel(int panelIndex)
    {
        HideAllUiPanels();
        if (panelIndex >= 0 && panelIndex < uiPanels.Length)
        {
            uiPanels[panelIndex].SetActive(true);
        }
        else
        {
            Debug.Log("panelIndex is out of reach: " + panelIndex);
        }
    }

    public void HideAllUiPanels()
    {
        foreach (var panel in uiPanels)
        {
            panel.SetActive(false);
        }
    }
}

