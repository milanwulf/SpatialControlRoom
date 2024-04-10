using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfDestroyer : MonoBehaviour
{
    [SerializeField] Button destroyButton;

    private void OnEnable()
    {
        destroyButton.onClick.AddListener(DestroyThisGameObject);
    }

    private void OnDisable()
    {
        destroyButton.onClick.RemoveListener(DestroyThisGameObject);
    }

    private void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }
}
