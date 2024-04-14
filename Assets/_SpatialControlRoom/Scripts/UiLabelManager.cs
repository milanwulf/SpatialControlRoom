using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLabelManager : MonoBehaviour
{
    [SerializeField] private GameObject uiLabelPrefab;

    private List<GameObject> uiLabelInstancesList = new List<GameObject>();

    public void InstatiateNewLabel(Vector3 position, Quaternion rotation)
    {
        GameObject newLabelInstance = Instantiate(uiLabelPrefab, position, rotation);
        uiLabelInstancesList.Add(newLabelInstance);
    }

    public void RemoveLabelInstance(GameObject uiLabelInstance)
    {
        uiLabelInstancesList.Remove(uiLabelInstance);
        Destroy(uiLabelInstance);
    }
}
