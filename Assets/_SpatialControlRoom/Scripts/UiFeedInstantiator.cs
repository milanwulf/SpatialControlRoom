using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flexalon;

public class UiFeedInstantiator : MonoBehaviour
{
    [SerializeField] BoxCollider protectedArea = null;
    [SerializeField] GameObject prefabToInstantiate;

    private FlexalonInteractable flexalonInteractable;

    private void Awake()
    {
        flexalonInteractable = GetComponent<FlexalonInteractable>();
    }

    private void Start()
    {
        if(protectedArea == null)
        {
            protectedArea = GameObject.Find("ProtectedArea").GetComponent<BoxCollider>();
        }
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
        InstantiateNewFeed();
    }

    private void InstantiateNewFeed()
    {
        Vector3 currentPos = transform.position;
        if (!IsPosInsideProtectedArea(currentPos))
        {
            Instantiate(prefabToInstantiate, currentPos, Quaternion.identity);
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
}
