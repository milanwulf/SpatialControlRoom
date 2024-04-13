using Flexalon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class LazyFollowController : MonoBehaviour
{
    [SerializeField] LazyFollow lazyFollow;
    [SerializeField] float renableDelay = 0.5f;

    private void OnEnable()
    {
        var flexalonInteractables = GetComponentsInChildren<FlexalonInteractable>();
        foreach (var flexalonInteractable in flexalonInteractables)
        {
            flexalonInteractable.DragStart.AddListener(DisableLazyFollower);
            flexalonInteractable.DragEnd.AddListener(EnableLazyFollower);
        }
    }

    private void OnDisable()
    {
        var flexalonInteractables = GetComponentsInChildren<FlexalonInteractable>();
        foreach (var flexalonInteractable in flexalonInteractables)
        {
            flexalonInteractable.DragStart.RemoveListener(DisableLazyFollower);
            flexalonInteractable.DragEnd.RemoveListener(EnableLazyFollower);
        }
    }

    private void DisableLazyFollower(FlexalonInteractable flexalonInteractable)
    {
        lazyFollow.enabled = false;
        Debug.Log("DisableLazyFollower");
    }

    private void EnableLazyFollower(FlexalonInteractable flexalonInteractable)
    {
        StartCoroutine(EnableLazyFollowerWithDelay());
    }

    private IEnumerator EnableLazyFollowerWithDelay()
    {
        yield return new WaitForSeconds(renableDelay);
        lazyFollow.enabled = true;
        Debug.Log("EnableLazyFollower");
    }
}
