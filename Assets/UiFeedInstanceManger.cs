using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFeedInstanceManger : MonoBehaviour
{
    [SerializeField] GameObject uiFeedPrefab;
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

    public void InstantiateNewFeed(Vector3 position, RenderTexture inputRenderTexture, Rect renderTextureOffset)
    {
        GameObject newFeedInstance = Instantiate(uiFeedPrefab, position, Quaternion.identity);
        UiFeed uiFeedInstance = newFeedInstance.GetComponent<UiFeed>();
        if(uiFeedInstance != null)
        {
            uiFeedInstance.SetRenderTexture(inputRenderTexture, renderTextureOffset);
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
}
