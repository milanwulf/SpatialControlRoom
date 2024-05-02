using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFeedInstanceManger : MonoBehaviour
{
    [SerializeField] GameObject uiFeedPrefab;
    [SerializeField] float duplicatePosOffset = 0.1f;
    [SerializeField] bool debugMode = false;

    private List<UiFeed> uiFeedInstancesList = new List<UiFeed>();

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {
            Debug.Log("UiFeedInstanceManger: " + uiFeedInstancesList.Count);
        }
    }

    public void InstantiateNewFeed(Vector3 position, Quaternion rotation, RenderTexture inputRenderTexture, Rect renderTextureOffset, UiFeed.FeedType feedType, string sceneName = null, int sceneId = 0, UiFeed.SceneState localSceneState = UiFeed.SceneState.isNotActive)
    {
        GameObject newFeedInstance = Instantiate(uiFeedPrefab, position, rotation);
        UiFeed uiFeedInstance = newFeedInstance.GetComponent<UiFeed>();
        if(uiFeedInstance != null)
        {
            uiFeedInstance.SetRenderTexture(inputRenderTexture, renderTextureOffset);
            uiFeedInstance.SetSceneIdAndType(sceneId, sceneName, feedType);
            uiFeedInstance.SetSceneState(localSceneState);
            uiFeedInstancesList.Add(uiFeedInstance);
        }
    }

    public void RemoveFeedInstance(UiFeed uiFeedInstance)
    {
        uiFeedInstancesList.Remove(uiFeedInstance);
        Destroy(uiFeedInstance.gameObject);
    }

    public void RegisterUiFeedInstance(UiFeed uiFeedInstance) //called by UiFeed if already in scene
    {
        if (!uiFeedInstancesList.Contains(uiFeedInstance))
        {
            uiFeedInstancesList.Add(uiFeedInstance);
        }
    }

    public void DuplicateUiFeedInstance(UiFeed uiFeedToDuplicate)
    {
        // Calculate the negative offset in the forward direction of the original UI feed
        Vector3 offset = -uiFeedToDuplicate.transform.forward * duplicatePosOffset; // This applies a negative offset
        Vector3 duplicatePosition = uiFeedToDuplicate.transform.position + offset;

        Quaternion duplicateRotation = uiFeedToDuplicate.transform.rotation;
        var instanceData = uiFeedToDuplicate.GetInstanceData();
        InstantiateNewFeed(duplicatePosition, duplicateRotation, instanceData.RenderTexture, instanceData.Offset, instanceData.FeedType, instanceData.SceneName, instanceData.SceneId, instanceData.SceneState);
    }



    //OBS Logic
    public void UpdateUiFeedScene(int index, string callingMethod) //called by the OBSWebSocketManager
    {
        if (uiFeedInstancesList.Count > 0)
        {
            foreach (var uiFeedInstance in uiFeedInstancesList)
            {
                if(uiFeedInstance.feedType == UiFeed.FeedType.Scene)
                {
                    if(uiFeedInstance.localSceneId == index)
                    {
                        if(callingMethod == "CurrentPreviewSceneChanged")
                        {
                            uiFeedInstance.SetSceneState(UiFeed.SceneState.isCurrentPreview);
                            //Debug.Log("Set all Panels to Preview with Index: " + index);
                        }
                        else if(callingMethod == "CurrentProgramSceneChanged")
                        {
                            uiFeedInstance.SetSceneState(UiFeed.SceneState.isCurrentProgram);
                            //Debug.Log("Set all Panels to Program with Index: " + index);
                        }
                    }
                    else
                    {
                     
                        if (callingMethod == "CurrentPreviewSceneChanged" && uiFeedInstance.localSceneState != UiFeed.SceneState.isCurrentProgram)
                        {
                            //Debug.Log("Scene name: " + uiFeedInstance.localSceneId + "Scene state" + uiFeedInstance.sceneState);
                            uiFeedInstance.SetSceneState(UiFeed.SceneState.isNotActive);
                        }
                        
                    }
                }
            }
        }
    }
}