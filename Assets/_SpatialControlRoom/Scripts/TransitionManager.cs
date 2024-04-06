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
        leftScissorPose.WhenSelected.AddListener(TriggerObsTransition);
        rightScissorPose.WhenSelected.AddListener(TriggerObsTransition);
    }

    private void OnDisable()
    {
        leftScissorPose.WhenSelected.RemoveListener(TriggerObsTransition);
        rightScissorPose.WhenSelected.RemoveListener(TriggerObsTransition);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            TriggerObsTransition();
        }
    }

    private void TriggerObsTransition()
    {
        Debug.Log("Transition should be triggered");
        obsWebSocketManager.TriggerStudioModeTransition();
    }
}
