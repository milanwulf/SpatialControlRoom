using Flexalon;
using Klak.Ndi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class UiInputSelectionPanel : MonoBehaviour
{
    [SerializeField] Button closeBtn;

    //For enabling/disabling NDI Feed InputSelectionObjects
    [Header("Just for enabling or disabling the Inputs if a NdiReceiver is disabled")]
    //[SerializeField] NdiManager ndiManager;
    [Header ("NDI Receivers")]
    [SerializeField] NdiReceiver ndiReceiver1;
    [SerializeField] NdiReceiver ndiReceiver2;
    [SerializeField] NdiReceiver ndiReceiver3;
    [SerializeField] GameObject noInputsNotification;

    [Header("NDI Feed Input Selection Objects")]
    [SerializeField] GameObject[] ndiFeed1Objects;
    [SerializeField] GameObject[] ndiFeed2Objects;
    [SerializeField] GameObject[] ndiFeed3Objects;

    [Header("LazyFollowDisabler")]
    [SerializeField] LazyFollow lazyFollow;
    [SerializeField] float renableDelay = 0.5f;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(ClosePanel);
        //ndiManager.onNdiReceiverStateChanged.AddListener(UpdateInputSelectionObjects);
        UpdateInputSelectionStates();

        //lazyFollow Disabler
        var flexalonInteractables = GetComponentsInChildren<FlexalonInteractable>();
        foreach (var flexalonInteractable in flexalonInteractables)
        {
            flexalonInteractable.DragStart.AddListener(DisableLazyFollower);
            flexalonInteractable.DragEnd.AddListener(EnableLazyFollower);
        }
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(ClosePanel);
        //ndiManager.onNdiReceiverStateChanged.RemoveListener(UpdateInputSelectionObjects);

        //lazyFollow Disabler
        var flexalonInteractables = GetComponentsInChildren<FlexalonInteractable>();
        foreach (var flexalonInteractable in flexalonInteractables)
        {
            flexalonInteractable.DragStart.RemoveListener(DisableLazyFollower);
            flexalonInteractable.DragEnd.RemoveListener(EnableLazyFollower);
        }
    }

    private void Start()
    {
        //Invoke(nameof(UpdateInputSelectionStates), 1f); //makes sure that the NDI Receivers are initialized by the NdiManager
    }

    private void ClosePanel()
    {
        lazyFollow.enabled = true;
        gameObject.SetActive(false);
    }

    public void ClosePanelWithDelay()
    {
        Invoke(nameof(ClosePanel), 0.2f);
    }

    private void UpdateInputSelectionStates() 
    {
        UpdateInputSelectionObjects(1, ndiReceiver1.enabled);
        UpdateInputSelectionObjects(2, ndiReceiver2.enabled);
        UpdateInputSelectionObjects(3, ndiReceiver3.enabled);
    }

    private void UpdateInputSelectionObjects(int ndiReceiverId, bool isActive)
    {
        Debug.Log("UpdateInputSelectionObjects: " + ndiReceiverId + " " + isActive);
        switch (ndiReceiverId)
        {
            case 1:
                foreach (var obj in ndiFeed1Objects)
                {
                    obj.gameObject.GetComponent<FlexalonObject>().SkipLayout = !isActive;
                    noInputsNotification.SetActive(!isActive);
                    obj.SetActive(isActive);
                }
                break;
            case 2:
                foreach (var obj in ndiFeed2Objects)
                {
                    obj.gameObject.GetComponent<FlexalonObject>().SkipLayout = !isActive;
                    obj.SetActive(isActive);
                }
                break;
            case 3:
                foreach (var obj in ndiFeed3Objects)
                {
                    obj.gameObject.GetComponent<FlexalonObject>().SkipLayout = !isActive;
                    obj.SetActive(isActive);
                }
                break;
        }
    }

    #region LazyFollowDisabler
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
    #endregion
}
