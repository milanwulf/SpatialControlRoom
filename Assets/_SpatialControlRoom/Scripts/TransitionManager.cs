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
    [SerializeField] private float poseCooldown = 0.2f;
    private float lastPoseTime;

    private void OnEnable()
    {
        leftScissorPose.WhenUnselected.AddListener(TriggerObsTransitionWithCooldown);
        rightScissorPose.WhenUnselected.AddListener(TriggerObsTransitionWithCooldown);
    }

    private void OnDisable()
    {
        leftScissorPose.WhenUnselected.RemoveListener(TriggerObsTransitionWithCooldown);
        rightScissorPose.WhenUnselected.RemoveListener(TriggerObsTransitionWithCooldown);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            TriggerObsTransition();
        }
    }

    private void TriggerObsTransitionWithCooldown() 
    { 
        if (Time.time - lastPoseTime > poseCooldown)
        {
            TriggerObsTransition();
            lastPoseTime = Time.time;
        }
    }

    private void TriggerObsTransition()
    {
        Debug.Log("Transition should be triggered");
        obsWebSocketManager.TriggerStudioModeTransition();
    }
}
