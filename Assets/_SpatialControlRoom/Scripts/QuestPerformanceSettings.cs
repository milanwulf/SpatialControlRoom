using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPerformanceSettings : MonoBehaviour
{
    //See https://developer.oculus.com/resources/os-cpu-gpu-levels#setting-cpu-and-gpu-levels
    //See https://developer.oculus.com/documentation/unity/po-quest-boost/
    //for more information on CPU and GPU levels

    // Start is called before the first frame update
    
    void Awake()
    {
        OVRManager.suggestedCpuPerfLevel = OVRManager.ProcessorPerformanceLevel.Boost;
        OVRManager.suggestedGpuPerfLevel = OVRManager.ProcessorPerformanceLevel.SustainedLow;
    }
    void Start()
    {
        Debug.Log("SPATIAL CONTROL ROOM Suggested CPU Performance Level: " + OVRManager.suggestedCpuPerfLevel);
        Debug.Log("SPATIAL CONTROL ROOM Suggested GPU Performance Level: " + OVRManager.suggestedGpuPerfLevel);

        //Depricated Method for setting CPU and GPU levels
        //OVRManager.cpuLevel = 5;
    }
}
