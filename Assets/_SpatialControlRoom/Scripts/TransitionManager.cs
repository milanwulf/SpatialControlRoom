using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private OBSWebSocketManager obsWebSocketManager;

    [Header("Hand Poses")]
    [SerializeField] private SelectorUnityEventWrapper leftScissorPose;
    [SerializeField] private SelectorUnityEventWrapper rightScissorPose;

    private void OnEnable()
    {
        leftScissorPose.WhenUnselected.AddListener(TriggerObsTransition);
        rightScissorPose.WhenUnselected.AddListener(TriggerObsTransition);
    }

    private void OnDisable()
    {
        leftScissorPose.WhenUnselected.RemoveListener(TriggerObsTransition);
        rightScissorPose.WhenUnselected.RemoveListener(TriggerObsTransition);
    }

    private void Update()
    {
        //check active controllers
        OVRInput.Controller activeController = OVRInput.GetActiveController();

        //only react to controller button presses and not hands
        if (activeController == OVRInput.Controller.Touch)
        {
            if(OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.A))
            {
                Debug.Log("OVRInput GetDown is triggerd");
                TriggerObsTransition();
            }
        }
    }

    private void TriggerObsTransition()
    {
        Debug.Log("Transition should be triggered");
        obsWebSocketManager.TriggerStudioModeTransition();
    }
}
