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
    [SerializeField] private UiFeedbackVisual feedbackVisual;
    [SerializeField] private Transform leftHandAnchorTransform; //also gets L Controller position
    [SerializeField] private Transform rightHandAnchorTransform; //also gets R Controller position

    private void OnEnable()
    {
        leftScissorPose.WhenUnselected.AddListener(() => TriggerObsTransition(leftHandAnchorTransform));
        rightScissorPose.WhenSelected.AddListener(() => TriggerObsTransition(rightHandAnchorTransform));
    }

    private void OnDisable()
    {
        leftScissorPose.WhenUnselected.RemoveListener(() => TriggerObsTransition(leftHandAnchorTransform));
        rightScissorPose.WhenSelected.RemoveListener(() => TriggerObsTransition(rightHandAnchorTransform));
    }

    private void Update()
    {
        //check active controllers
        OVRInput.Controller activeController = OVRInput.GetActiveController();

        //only react to controller button presses and not hands
        if (activeController == OVRInput.Controller.RTouch || activeController == OVRInput.Controller.LTouch || activeController == OVRInput.Controller.Touch) //checks if only R or L controller is active or both
        {
            if (OVRInput.GetDown(OVRInput.RawButton.X))
            {
                TriggerObsTransition(leftHandAnchorTransform);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                TriggerObsTransition(rightHandAnchorTransform);
            }
        }
    }

    private void TriggerObsTransition(Transform spawnLocation)
    {
        Debug.Log("Transition should be triggered");
        obsWebSocketManager.TriggerStudioModeTransition();
        feedbackVisual.TriggerVisualFeedback("cut", spawnLocation);
    }
}
