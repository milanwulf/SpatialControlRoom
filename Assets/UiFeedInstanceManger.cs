using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFeedInstanceManger : MonoBehaviour
{
    [SerializeField] GameObject uiFeedPrefab;
    [SerializeField] Vector3 dublicatePosOffset = new Vector3(0f, 0f, -0.2f);
    [SerializeField] bool debugMode = false;

    private List<UiFeed> uiFeedInstances = new List<UiFeed>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            Debug.Log("UiFeedInstanceManger: " + uiFeedInstances.Count);
        }
    }

    public void InstantiateNewFeed(Vector3 position, Quaternion rotation, RenderTexture inputRenderTexture, Rect renderTextureOffset, UiFeed.FeedType feedType, string sceneName = null)
    {
        GameObject newFeedInstance = Instantiate(uiFeedPrefab, position, rotation);
        UiFeed uiFeedInstance = newFeedInstance.GetComponent<UiFeed>();
        if(uiFeedInstance != null)
        {
            uiFeedInstance.SetRenderTexture(inputRenderTexture, renderTextureOffset);
            uiFeedInstance.SetSceneIdAndType(sceneName, feedType);
            uiFeedInstances.Add(uiFeedInstance);
        }
    }

    public void RemoveFeedInstance(UiFeed uiFeedInstance)
    {
        uiFeedInstances.Remove(uiFeedInstance);
        Destroy(uiFeedInstance.gameObject);
    }

    public void RegisterUiFeedInstance(UiFeed uiFeedInstance) //called by UiFeed if already in scene
    {
        if (!uiFeedInstances.Contains(uiFeedInstance))
        {
            uiFeedInstances.Add(uiFeedInstance);
        }
    }

    public void DublicateUiFeedInstance(UiFeed uiFeedToDuplicate)
    {
        Vector3 dublicatePosition = uiFeedToDuplicate.transform.position + dublicatePosOffset;
        Quaternion dublicateRotation = uiFeedToDuplicate.transform.rotation;
        var instanceData = uiFeedToDuplicate.GetInstanceData();
        InstantiateNewFeed(dublicatePosition, dublicateRotation, instanceData.RenderTexture, instanceData.Offset, instanceData.FeedType, instanceData.SceneID);
    }

    //OBS Logic
    private void UpdateUiFeedScene()
    {
        if (uiFeedInstances.Count > 0)
        {
            foreach (var uiFeedInstance in uiFeedInstances)
            {
                if(uiFeedInstance.feedType == UiFeed.FeedType.Scene)
                {
                    //TODO: add if elese logic
                    uiFeedInstance.SetSceneState(UiFeed.SceneState.isCurrentPreview);
                    uiFeedInstance.SetSceneState(UiFeed.SceneState.isCurrentProgram);
                    uiFeedInstance.SetSceneState(UiFeed.SceneState.isNotActive);
                }
            }
        }
    }
}