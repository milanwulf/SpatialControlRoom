using Klak.Ndi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NdiManager : MonoBehaviour
{
    [Header("NDI Receiver")]
    [SerializeField] private NdiReceiver ndiReceiver1;
    [SerializeField] private NdiReceiver ndiReceiver2;
    [SerializeField] private NdiReceiver ndiReceiver3;

    private List<string> ndiSourceNames;

    [Header("Default NDI Source Names")]
    [SerializeField] private string defaultSourceName1 = "Feed1_Group";
    [SerializeField] private string defaultSourceName2 = "Feed2_Group";
    [SerializeField] private string defaultSourceName3 = "Feed3_Group";
    private bool defaultSourcesSet = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(GetAvailableNdiFeeds), 3f);
    }
    public void GetAvailableNdiFeeds() //Important: call with a short delay on Start() to get all NDI sources, otherwise it will return just one source
    {
        ndiSourceNames = NdiFinder.sourceNames.ToList();
        Debug.Log("Found " + ndiSourceNames.Count + " NDI sources.");
        foreach (var sourceName in ndiSourceNames)
        {
            Debug.Log("Avaiable NDI Source: " + sourceName);
        }
        
        if(!defaultSourcesSet)
        {
            SetDefaults();
            defaultSourcesSet = true;
        }
    }
    private void SetDefaults()
    {
        // Checking if the list is empty
        if(ndiSourceNames == null || ndiSourceNames.Count == 0)
        {
            Debug.LogWarning($"The {nameof(ndiSourceNames)} List is empty, not able to set defaults");
            return;
        }

        // Flags to check if defaults are set
        bool default1Set = false, default2Set = false, default3Set = false;

        // Setting default NDI source names
        for( int i = 0; i < ndiSourceNames.Count; i++)
        {
            if (ndiSourceNames[i].Contains(defaultSourceName1))
            {
                Debug.Log("Found default NDI source name 1: " + defaultSourceName1);    
                ndiReceiver1.ndiName = ndiSourceNames[i];
                default1Set = true;
            }
            else if (ndiSourceNames[i].Contains(defaultSourceName2))
            {
                Debug.Log("Found default NDI source name 2: " + defaultSourceName2);
                ndiReceiver2.ndiName = ndiSourceNames[i];
                default2Set = true;
            }
            else if (ndiSourceNames[i].Contains(defaultSourceName3))
            {
                Debug.Log("Found default NDI source name 3: " + defaultSourceName3);
                ndiReceiver3.ndiName = ndiSourceNames[i];
                default3Set = true;
            }
        }

        // Checking if any of the defaults were not set
        if (!default1Set)
        {
            Debug.LogWarning("Default NDI source name 1 not found: " + defaultSourceName1);
        }
        if (!default2Set)
        {
            Debug.LogWarning("Default NDI source name 2 not found: " + defaultSourceName2);
        }
        if (!default3Set)
        {
            Debug.LogWarning("Default NDI source name 3 not found: " + defaultSourceName3);
        }
    }
    public void SetNdiReceiverSource(int receiverId, int value)
    {
        // Checking if the list is empty
        if(ndiSourceNames == null || ndiSourceNames.Count == 0)
        {
            Debug.LogError($"The {nameof(ndiSourceNames)} List is empty, call {nameof(GetAvailableNdiFeeds)} first!");
            return;
        }

        // Check if the passed index is within the valid range of the list
        if (value < 0 || value >= ndiSourceNames.Count)
        {
            Debug.LogError("Index out of range. Please select a valid NDI source.");
            return;
        }

        // Set the NDI source name for the receiver
        switch (receiverId)
        {
            case 1:
                ndiReceiver1.ndiName = ndiSourceNames[value];
                break;
            case 2:
                ndiReceiver2.ndiName = ndiSourceNames[value];
                break;
            case 3:
                ndiReceiver3.ndiName = ndiSourceNames[value];
                break;
            default:
                Debug.LogError("Invalid receiver id. Please select a valid NDI receiver.");
                break;
        }
    }
    public List<string> GetNdiSourceNames()
    {
        return ndiSourceNames;
    }
    public string GetCurrentNdiSourceName(int receiverId)
    {
        switch (receiverId)
        {
            case 1:
                return ndiReceiver1.ndiName;
            case 2:
                return ndiReceiver2.ndiName;
            case 3:
                return ndiReceiver3.ndiName;
            default:
                Debug.LogError("Invalid receiver id.");
                return null;
        }
    }
}
