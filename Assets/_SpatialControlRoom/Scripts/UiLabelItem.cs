using Flexalon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlexalonInteractable))]

public class UiLabelItem : MonoBehaviour
{
    [SerializeField] private UiLabelManager uiLabelManager = null;
    [SerializeField] private BoxCollider protectedArea = null;
    private FlexalonInteractable flexalonInteractable;
    private UiInputSelectionPanel uiInputSelectionPanel;

    private void Awake()
    {
        flexalonInteractable = GetComponent<FlexalonInteractable>();
        uiInputSelectionPanel = GetComponentInParent<UiInputSelectionPanel>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (uiLabelManager != null)
        {
            uiLabelManager = GameObject.Find("UiLabelManager").GetComponent<UiLabelManager>();
        }

        if (protectedArea == null)
        {
            protectedArea = GetComponent<BoxCollider>();
        }
    }
    
    private void OnEnable()
    {
       flexalonInteractable.DragEnd.AddListener(OnDragEnd);
    }

    private void OnDisable()
    {
        flexalonInteractable.DragEnd.RemoveListener(OnDragEnd);
    }

    private void OnDragEnd(FlexalonInteractable interactable)
    {
        Vector3 currentPos = transform.position;

       if(!IsInProtectedArea(currentPos))
       {
            Vector3 currentEuler = transform.rotation.eulerAngles;
            Quaternion newRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, 0);
            uiLabelManager.InstatiateNewLabel(currentPos, newRotation);
            uiInputSelectionPanel.ClosePanelWithDelay();
        }
    }

    private bool IsInProtectedArea(Vector3 position)
    {
        if(protectedArea == null)
        {
            return protectedArea.bounds.Contains(position);
        }
        return false;
    }
}
